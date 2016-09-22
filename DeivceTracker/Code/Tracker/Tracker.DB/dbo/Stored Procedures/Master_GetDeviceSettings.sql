CREATE PROCEDURE [dbo].[Master_GetDeviceSettings] 
(
 @DeviceId NVARCHAR(50) = NULL
)
AS
BEGIN
  
 SELECT 
  DM.[DeviceId]
  ,DM.[VehicleNo]
  ,DM.[IMEINo]
  ,DM.[PrimaryMobile]
  ,DM.[SecondaryMobile]
  ,DM.[Make]
  ,DM.[VehicleModel]
  ,DM.[VehicleType]
  ,DM.[Mail]
  ,DM.[SimNetwork]
  ,DM.[DeviceSimNo]
  ,DM.[Status]
  ,DM.[EntryDate]
  ,DM.[TimeZone]
  ,DM.[ToExpiry]
  ,DM.[ExpiryDate]
  ,DM.[UserId]
  ,DM.[DeviceType]
  ,ISNULL(DCD.Odometer, 0) AS Odometer
 FROM [DeviceModels] DM
 LEFT JOIN
  DeviceCalcData DCD
 ON DM.IMEINo = DCD.DeviceId 
 WHERE
  DM.IMEINo = ISNULL(@DeviceId, DM.IMEINo) 
 
END