CREATE PROCEDURE [dbo].[A_GetFiredAlertReceivers]
(
	@DeviceAlertId INT
)
AS
BEGIN
	
	SELECT
		Id,
		DeviceAlertId,
		[To],
		MediumType,
		CreatedDate,
		ModifiedDate
	FROM DeviceAlertReceivers
	WHERE
		DeviceAlertId = @DeviceAlertId
END