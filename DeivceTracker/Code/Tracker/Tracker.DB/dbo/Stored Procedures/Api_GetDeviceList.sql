CREATE PROCEDURE [dbo].[Api_GetDeviceList]
(
	@UserId UNIQUEIDENTIFIER
)
AS
BEGIN

	DECLARE @TBLUsers TABLE(
		UserId UNIQUEIDENTIFIER
	);
	
	INSERT INTO @TBLUsers
	SELECT UserId FROM [User]
	WHERE	
		UserId = @UserId
	OR
		Parent = @UserId
	
	
	
	SELECT
		IMEINo AS DeviceId,
		VehicleNo,
		IMEINo,
		PrimaryMobile,
		SecondaryMobile,
		Make,
		VehicleModel,
		VehicleType,
		Mail,
		SimNetwork,
		DeviceSimNo,
		--Status,
		TimeZone,
		ExpiryDate,
		--UserId,
		DeviceType,
		EntryDate
	FROM DeviceModels
	WHERE
		UserId IN (SELECT UserId FROM @TBLUsers);
		
END