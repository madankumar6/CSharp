--EXEC Report_Ignition '2016-07-01 00:00:00', '2016-09-01 00:00:00', '0358899055875739'
CREATE PROCEDURE [dbo].[Report_Ignition]
(
	@StartDate DATETIME = NULL,
	@EndDate DATETIME = NULL,
	@DeviceId NVARCHAR(50) = NULL,
	@UserId NVARCHAR(32) = NULL
)
AS
BEGIN

	DECLARE @OUTTempHistory TABLE(
		RN INT,
		OnAcc INT,
		StartDate DATETIME,
		StopDate DATETIME,
		Duration INT,
		OdoMeter INT,
		StartLatitude NVARCHAR(50),
		StartLongitude NVARCHAR(50),
		StopLatitude NVARCHAR(50),
		StopLongitude NVARCHAR(50)
	);

	IF OBJECT_ID('tempdb..#TempHistory1') IS NOT NULL
		TRUNCATE TABLE #TempHistory1
	ELSE
	BEGIN
		CREATE TABLE #TempHistory1(
			RN INT,
			OnAcc INT,
			OdoMeter INT,
			ActionTime DATETIME,
			Latitude NVARCHAR(50),
			Longitude NVARCHAR(50)
		)
	END

	INSERT INTO #TempHistory1
	(
		RN,
		OnAcc,
		OdoMeter,
		ActionTime,
		Latitude,
		Longitude 
	)
	SELECT
		ROW_NUMBER() OVER(ORDER BY ActionTime) AS RN,
		OnAcc,
		ISNULL(OdoMeter, 0),
		ActionTime,
		Latitude,
		Longitude
	FROM [dbo].[DeviceData_History] rt
	--WHERE ActionTime between '2016-07-01' and '2016-08-01 12:00:00'
	WHERE ActionTime BETWEEN @StartDate AND @EndDate
	AND OnAcc IS NOT NULL
	AND DeviceId = @DeviceId


	;WITH CTE AS (
		SELECT RN, OnAcc, OdoMeter, Latitude, Longitude, ActionTime,
		ROW_NUMBER() OVER(PARTITION BY OnAcc ORDER BY ActionTime) AS tGN,
		ROW_NUMBER() OVER (ORDER BY ActionTime) AS tRN
	FROM #TempHistory1
	)	
	, CTE2 AS (
		SELECT RN, OnAcc, OdoMeter, Latitude, Longitude, ActionTime, tGN, tRN, tGN - tRN  AS tGB,
		0 AS tGN1
		--ROW_NUMBER() OVER(PARTITION BY (tGN - tRN) ORDER BY RN) AS tGN1
		FROM CTE 
	)
	, CTE3 AS (
		SELECT MIN(RN) StartRow, MAX(RN) EndRow, MIN(OnAcc) OnAcc, SUM(OdoMeter) TotalOdoMeter
		 --Latitude, Longitude, ActionTime, tGN, tRN, tGB
		FROM CTE2
		GROUP BY tGB
	)
	, CTE4 AS (
		SELECT
			CTEINNER2.RN RN,
			CTEINNER2.ActionTime StartTime,
			CTEINNER21.ActionTime EndTime,
			
			CTEINNER2.Latitude StartLatitude,
			CTEINNER2.Longitude StartLongitude,
			
			CTEINNER21.Latitude EndLatitude,
			CTEINNER21.Longitude EndLongitude,
			
			CTE3.OnAcc, CTE3.TotalOdoMeter
		FROM CTE3 INNER JOIN CTE2 CTEINNER2
		ON CTE3.StartRow = CTEINNER2.RN
		INNER JOIN CTE2 CTEINNER21
		ON CTE3.EndRow = CTEINNER21.RN
	)
	INSERT INTO @OUTTempHistory(
		RN,
		OnAcc,
		StartDate,
		StopDate,
		Duration,
		OdoMeter,
		StartLatitude,
		StartLongitude,
		StopLatitude,
		StopLongitude
	)
	SELECT 
		RN,
		OnAcc,
		StartTime,
		EndTime,
		DATEDIFF(MINUTE, StartTime, EndTime),
		TotalOdoMeter,
		StartLatitude,
		StartLongitude,
		EndLatitude,
		EndLongitude		
	FROM CTE4
	WHERE CTE4.OnAcc <> 0
	
	SELECT 
		RN,
		OnAcc,
		StartDate,
		StopDate,
		Duration,
		OdoMeter,
		StartLatitude,
		StartLongitude,
		StopLatitude,
		StopLongitude
	FROM @OUTTempHistory		
	ORDER BY RN ASC
	
END