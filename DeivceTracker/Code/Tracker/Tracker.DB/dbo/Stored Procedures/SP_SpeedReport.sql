CREATE procedure [dbo].[SP_SpeedReport]

 --@DeviceAlertId numeric,
 @StartDate DateTime,
 @EndDate DateTime,
 @Speed numeric
AS
BEGIN
 SELECT
 DeviceId,
  Latitude,
  Longitude,
  Speed,
  DeviceDataTime
 
 FROM
  [DeviceData_History]
WHERE
  (DeviceDataTime BETWEEN @StartDate AND @EndDate) AND (Speed >= @Speed)



END