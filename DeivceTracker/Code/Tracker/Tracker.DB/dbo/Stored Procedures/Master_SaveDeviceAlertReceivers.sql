CREATE PROCEDURE [dbo].[Master_SaveDeviceAlertReceivers](
	@DeviceAlertId INT,
	@To NVARCHAR(300),
	@MediumType INT,
	@Id INT = NULL,
	@IsToDelete BIT = NULL
)
AS
BEGIN
	IF(@IsToDelete IS NOT NULL AND @IsToDelete = 1)
	BEGIN
		DELETE FROM DeviceAlertReceivers WHERE Id = @Id;
	END
	ELSE
	BEGIN
		IF EXISTS(SELECT TOP 1 1 FROM DeviceAlertReceivers WHERE Id = @Id)
		BEGIN
			UPDATE DeviceAlertReceivers
			SET
				DeviceAlertId = @DeviceAlertId,
				[To] = @To,
				MediumType = @MediumType,
				ModifiedDate = GETUTCDATE()
			WHERE
				Id = @Id
		END
		ELSE
		BEGIN
		
			INSERT INTO DeviceAlertReceivers(
				DeviceAlertId,
				[To],
				MediumType,
				CreatedDate,
				ModifiedDate
			)
			VALUES(
				@DeviceAlertId,
				@To,
				@MediumType,
				GETUTCDATE(),
				GETUTCDATE()
			)
		
		END
	END
END