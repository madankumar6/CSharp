create PROCEDURE [dbo].[Master_SaveDeviceSettings] 
(
 @DeviceId NVARCHAR(50),
 @Odometer INT = NULL
)
AS
BEGIN

 UPDATE DeviceCalcData
 SET
  Odometer = @Odometer
 WHERE
  DeviceId = @DeviceId
   
END