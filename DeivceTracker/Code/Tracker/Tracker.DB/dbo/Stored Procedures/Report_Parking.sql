CREATE PROCEDURE Report_Parking
(
 @StartDate DATETIME,
 @EndDate DATETIME,
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
  StartLatitude NVARCHAR(50),
  StartLongitude NVARCHAR(50),
  StopLatitude NVARCHAR(50),
  StopLongitude NVARCHAR(50)
 );

 IF OBJECT_ID('tempdb..#TempHistory') IS NOT NULL
  TRUNCATE TABLE #TempHistory
 ELSE
 BEGIN
  CREATE TABLE #TempHistory(
   RN INT,
   OnAcc INT,
   ActionTime DATETIME,
   Latitude NVARCHAR(50),
   Longitude NVARCHAR(50)
  )
 END

 INSERT INTO #TempHistory
 (
  RN,
  OnAcc,
  ActionTime,
  Latitude,
  Longitude 
 )
 SELECT
  ROW_NUMBER() OVER(ORDER BY ActionTime) AS RN,
  OnAcc,
  ActionTime,
  Latitude,
  Longitude
 FROM [dbo].[DeviceData_History] rt
 --WHERE ActionTime between '2016-07-01' and '2016-08-01 12:00:00'
 WHERE ActionTime BETWEEN @StartDate AND @EndDate
 AND OnAcc IS NOT NULL
 --and DeviceId
 ;WITH a
 as
 (
  SELECT
   ROW_NUMBER() OVER(ORDER BY a1.ActionTime) AS RN1,
   a1.ActionTime ,
   a1.OnAcc,
   a1.Latitude StartLatitude,
   a1.Longitude StartLongitude,
   a2.ActionTime AT,
   a2.OnAcc OnAcc1,
   a2.Latitude StopLatitude,
   a2.Longitude StopLongitude
  FROM #TempHistory a1
  LEFT JOIN #TempHistory a2 ON a2.RN = a1.RN - 1
  WHERE
   (a1.OnAcc != a2.OnAcc)
   OR 
   (a2.RN IS NULL)
 )
 INSERT INTO @OUTTempHistory
 (
  OnAcc,
  StartDate,
  StopDate,
  Duration,
  StartLatitude,
  StartLongitude,
  StopLatitude,
  StopLongitude 
 )
 SELECT
  b1.OnAcc,
  b1.ActionTime AS StartDate,
  ISNULL(b2.ActionTime, @EndDate) AS EndDate,
  DATEDIFF(MINUTE, b1.ActionTime, ISNULL(b2.ActionTime, @EndDate)) Duration,
  b1.StartLatitude,
  b1.StartLongitude,
  b2.StartLatitude,
  b2.StartLongitude
  FROM a b1
  LEFT OUTER JOIN a b2 ON b2.RN1 = b1.RN1 + 1
  ORDER BY b1.ActionTime
 
 SELECT * FROM @OUTTempHistory
 WHERE OnAcc = 0 
END