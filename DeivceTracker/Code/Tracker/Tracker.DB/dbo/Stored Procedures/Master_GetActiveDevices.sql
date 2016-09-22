CREATE PROCEDURE [dbo].[Master_GetActiveDevices]
AS
BEGIN
	SELECT  DeviceId, DeviceId VehicleId from DeviceData
	WHERE DeviceId NOT IN (SELECT  DeviceId from UnknownDeviceData)
END