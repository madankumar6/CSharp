
CREATE PROCEDURE [dbo].[T_GetActiveDevices]
AS
BEGIN
	SELECT  DeviceId from UnknownDeviceData
END