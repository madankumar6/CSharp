CREATE PROCEDURE [dbo].[Api_GetDeviceCurrentData]
(
	@DeviceList NVARCHAR(4000)
)
AS
BEGIN



	SELECT * FROM DeviceData DA
	INNER JOIN fnc_Split(@DeviceList, ',') DL
	ON DA.DeviceId = DL.Data
	
	
END