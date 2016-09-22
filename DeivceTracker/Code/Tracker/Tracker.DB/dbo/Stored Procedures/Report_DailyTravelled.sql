--EXEC Report_DailyTravelled '2016-07-01 00:00:00', '2016-07-02 23:59:59', '0358899055875739'

--SELECT SUM(Odometer)/CAST(1000 AS FLOAT) FROM DeviceData_History 
--WHERE ActionTime BETWEEN '2016-07-01 08:32:05.260' AND '2016-07-01 16:39:40.453' 
--AND OnAcc = 1
--SELECT SUM(Odometer)/CAST(1000 AS FLOAT) FROM DeviceData_History 
--WHERE ActionTime BETWEEN '2016-07-02 00:16:33.277' AND '2016-07-02 16:59:46.220'
--AND OnAcc = 1

CREATE PROCEDURE [dbo].[Report_DailyTravelled]
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

	IF OBJECT_ID('tempdb..#TempDTravel1') IS NOT NULL
		TRUNCATE TABLE #TempDTravel1
	ELSE
	BEGIN
		CREATE TABLE #TempDTravel1(
			RN INT,
			OnAcc INT,
			OdoMeter INT,
			ActionTime DATETIME,
			ActionDate DATETIME,
			Latitude NVARCHAR(50),
			Longitude NVARCHAR(50)
		)
	END

	INSERT INTO #TempDTravel1
	(
		RN,
		OnAcc,
		OdoMeter,
		ActionTime,
		ActionDate,
		Latitude,
		Longitude 
	)
	SELECT
		ROW_NUMBER() OVER(ORDER BY ActionTime) AS RN,
		OnAcc,
		ISNULL(OdoMeter, 0),
		ActionTime,
		CONVERT(NVARCHAR(12), ActionTime, 103) AS ActionDate,
		Latitude,
		Longitude
	FROM [dbo].[DeviceData_History] rt
	--WHERE ActionTime between '2016-07-01' and '2016-08-01 12:00:00'
	WHERE ActionTime BETWEEN @StartDate AND @EndDate
	--AND OnAcc = 0
	AND DeviceId = @DeviceId

	;WITH CTE3 AS (
		SELECT MIN(RN) StartRow, MAX(RN) EndRow, MIN(OnAcc) OnAcc, SUM(OdoMeter) TotalOdoMeter
		 --Latitude, Longitude, ActionTime, tGN, tRN, tGB
		FROM #TempDTravel1
		GROUP BY ActionDate
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
		FROM CTE3 INNER JOIN #TempDTravel1 CTEINNER2
		ON CTE3.StartRow = CTEINNER2.RN
		INNER JOIN #TempDTravel1 CTEINNER21
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
	--WHERE CTE4.OnAcc <> 0
	
	SELECT 
		RN,
		--OnAcc,
		StartDate,
		StopDate,
		Duration,
		OdoMeter/CAST(1000 AS FLOAT) OdoMeter,
		StartLatitude,
		StartLongitude,
		StopLatitude,
		StopLongitude
	FROM @OUTTempHistory		
	ORDER BY RN ASC
	
END