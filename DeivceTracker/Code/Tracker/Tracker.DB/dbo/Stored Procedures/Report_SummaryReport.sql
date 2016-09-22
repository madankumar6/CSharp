--EXEC Report_SummaryReport '2016-07-01 00:00:00', '2016-07-02 23:59:59', '0358899055875739'

CREATE PROCEDURE [dbo].[Report_SummaryReport]
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

	IF OBJECT_ID('tempdb..#TempSummaryReport1') IS NOT NULL
		TRUNCATE TABLE #TempSummaryReport1
	ELSE
	BEGIN
		CREATE TABLE #TempSummaryReport1(
			RN INT,
			DeviceId NVARCHAR(50),
			OnAcc INT,
			OdoMeter INT,
			ActionTime DATETIME,
			Latitude NVARCHAR(50),
			Longitude NVARCHAR(50)
		)
	END

	INSERT INTO #TempSummaryReport1
	(
		RN,
		DeviceId,
		OnAcc,
		OdoMeter,
		ActionTime,
		Latitude,
		Longitude 
	)
	SELECT
		ROW_NUMBER() OVER(ORDER BY ActionTime) AS RN,
		DeviceId,
		OnAcc,
		ISNULL(OdoMeter, 0),
		ActionTime,
		Latitude,
		Longitude
	FROM [dbo].[DeviceData_History] rt
	--WHERE ActionTime between '2016-07-01' and '2016-08-01 12:00:00'
	WHERE ActionTime BETWEEN @StartDate AND @EndDate
	AND OnAcc IS NOT NULL
	AND DeviceId = ISNULL(@DeviceId, DeviceId)


	;WITH CTE AS (
		SELECT RN, DeviceId, OnAcc, OdoMeter, Latitude, Longitude, ActionTime,
		ROW_NUMBER() OVER(PARTITION BY OnAcc ORDER BY ActionTime) AS tGN,
		ROW_NUMBER() OVER (ORDER BY ActionTime) AS tRN
	FROM #TempSummaryReport1
	)	
	, CTE2 AS (
		SELECT RN, DeviceId, OnAcc, OdoMeter, Latitude, Longitude, ActionTime, tGN, tRN, tGN - tRN  AS tGB,
		0 AS tGN1
		FROM CTE 
	)
	, CTE3 AS (
		SELECT DeviceId, MIN(RN) StartRow, MAX(RN) EndRow, MIN(OnAcc) OnAcc, SUM(OdoMeter) TotalOdoMeter
		 --Latitude, Longitude, ActionTime, tGN, tRN, tGB
		FROM CTE2
		GROUP BY tGB, DeviceId
	)
	, CTE4 AS (
		SELECT
			CTEINNER2.RN RN,
			CTEINNER2.DeviceId,
						
			CTEINNER2.ActionTime StartTime,
			CTEINNER21.ActionTime EndTime,
			DATEDIFF(MINUTE, CTEINNER2.ActionTime, CTEINNER21.ActionTime) Duration,
			CONVERT(NVARCHAR(12), CTEINNER2.ActionTime, 103) ActionDate,
			
			CTEINNER2.Latitude StartLatitude,
			CTEINNER2.Longitude StartLongitude,
			
			CTEINNER21.Latitude EndLatitude,
			CTEINNER21.Longitude EndLongitude,
			
			CTE3.OnAcc, CTE3.TotalOdoMeter
		FROM CTE3 INNER JOIN CTE2 CTEINNER2
		ON	CTE3.DeviceId = CTEINNER2.DeviceId
		AND CTE3.StartRow = CTEINNER2.RN
		INNER JOIN CTE2 CTEINNER21
		ON	CTE3.DeviceId = CTEINNER21.DeviceId
		AND	CTE3.EndRow = CTEINNER21.RN
	)
	,CTEAdGroup AS(
		SELECT CTE4.DeviceId, 0 AS OnAcc, '' StartTime, '' EndTime, CTE4.ActionDate, COUNT(*) Cnt, SUM(CTE4.Duration) Duration 
		FROM CTE4
		WHERE CTE4.OnAcc = 0
		GROUP BY CTE4.DeviceId, CTE4.ActionDate
		
		UNION
		
		SELECT CTE4.DeviceId, 1 AS OnAcc, MIN(CTE4.StartTime) StartTime, MAX(CTE4.EndTime) EndTime, CTE4.ActionDate, COUNT(*) Cnt, SUM(CTE4.Duration) Duration 
		FROM CTE4
		WHERE CTE4.OnAcc = 1
		GROUP BY CTE4.DeviceId, CTE4.ActionDate
	)
	,CTEAdGroupJoin AS(
		SELECT
			L2.DeviceId,
			L2.ActionDate,
			L2.StartTime,
			L2.EndTime,
			L2.Cnt TripCount,
			L2.Duration TripDuration,
			L1.Cnt ParkingCount,
			L1.Duration ParkingDuration
		FROM CTEAdGroup L1 -- L1 as Parking
		INNER JOIN CTEAdGroup L2
		ON	L1.OnAcc <> L2.OnAcc
		AND L1.DeviceId = L2.DeviceId
		AND L1.ActionDate = L2.ActionDate
		--AND DEVICEID		
		WHERE L2.OnAcc = 1
	)
	SELECT
		DeviceId,
		ActionDate,
		StartTime,
		EndTime,
		TripCount,
		TripDuration,
		ParkingCount,
		ParkingDuration		
	FROM CTEAdGroupJoin
	ORDER BY StartTime  
	
END