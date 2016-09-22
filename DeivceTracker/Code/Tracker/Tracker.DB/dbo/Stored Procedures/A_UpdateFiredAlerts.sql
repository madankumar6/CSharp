CREATE PROCEDURE [dbo].[A_UpdateFiredAlerts]
(
	@ProcessorId INT
)
AS
BEGIN
	
	DELETE
	FROM
		DeviceFiredAlerts
	WHERE
		ProcessorId = @ProcessorId

END