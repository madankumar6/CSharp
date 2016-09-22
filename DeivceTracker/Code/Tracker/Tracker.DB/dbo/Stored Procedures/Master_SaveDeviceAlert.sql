CREATE PROCEDURE [dbo].[Master_SaveDeviceAlert](
	@DeviceAlertsId int,
	@DeviceId nvarchar(50) = NULL,
	@AlertType int,
	@Eval nvarchar(300) = NULL,
	@Operand nvarchar(50) = NULL,
	@Operator nvarchar(50) = NULL,
	@Value nvarchar(50) = NULL,
	@IsActive bit = NULL,
	@IsToDelete BIT = NULL,
	@Name nvarchar(150) = NULL,
	@DescriptionText nvarchar(150) = NULL
)
AS
BEGIN
	IF(@IsToDelete IS NOT NULL AND @IsToDelete = 1)
	BEGIN
		DELETE FROM [DeviceAlerts] WHERE Id = @DeviceAlertsId;
		
		DELETE FROM DeviceAlertReceivers WHERE DeviceAlertId = @DeviceAlertsId;
		
		SELECT -1;
		
		RETURN 0;
	END

	IF EXISTS(SELECT TOP 1 1 FROM [DeviceAlerts] WHERE Id = @DeviceAlertsId)
	BEGIN
	
		UPDATE [DeviceAlerts]
		SET
			[DeviceId] = @DeviceId,
			[AlertType] = @AlertType,
			[Eval] = @Eval,			
			[Operand] = @Operand,
			[Operator] = @Operator,
			[Value] = @Value,
			[IsActive] = @IsActive,
			[ModifiedDate] = GETUTCDATE(),
			[Name] = COALESCE(@Name, Name),
			[DescriptionText] = COALESCE(@DescriptionText, DescriptionText)
		WHERE
			[Id] = @DeviceAlertsId 
		
		SELECT @DeviceAlertsId;
	
	END
	ELSE
	BEGIN

		--DELETE FROM [DeviceAlerts]
		--WHERE Id = @DeviceAlertsId-- AND DeviceId = @DeviceId AND AlertType = @AlertType


		INSERT INTO [DeviceAlerts]
			   ([DeviceId]
			   ,[AlertType]
			   ,[Eval]			   
			   ,[Operand]
			   ,[Operator]
			   ,[Value]
			   ,[IsActive]
			   ,[CreatedDate]
			   ,[ModifiedDate]
			   ,[Name]
			   ,[DescriptionText]
			   )
		 VALUES
			   (
			   @DeviceId,
			   @AlertType,
			   @Eval,
			   @Operand,
			   @Operator,
			   @Value,
			   @IsActive,
			   GETUTCDATE(),
			   GETUTCDATE(),
			   @Name,
			   @DescriptionText)
			   
		SELECT SCOPE_IDENTITY();	   
	END
END