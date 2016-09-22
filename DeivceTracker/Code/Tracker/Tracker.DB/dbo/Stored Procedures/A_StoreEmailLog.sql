
CREATE PROCEDURE [dbo].[A_StoreEmailLog]
(
	@DeviceAlertId INT,
	@DeviceId NVARCHAR(50),
	@AlertType INT,
	@Email NVARCHAR(150) = NULL,
	@EmailSubject NVARCHAR(max) = NULL,
	@EmailPlainText NVARCHAR(max) = NULL,
	@EmailHtmlContent NVARCHAR(max) = NULL,
	@SmsSubject NVARCHAR(max) = NULL,
	@SmsPlainText NVARCHAR(max) = NULL,
	@MediumType INT = NULL,
	@SentStatus NVARCHAR(50) = NULL,
	@ErrorMessage NVARCHAR(1000) = NULL
)
AS
BEGIN
	
	INSERT INTO AlertEmailLog
        (
			DeviceAlertId,
			DeviceId,
			AlertType,
			Email,
			MediumType,
			SentStatus,
			ErrorMessage,
			EmailSubject,
			EmailPlainText,
			EmailHtmlContent,
			SmsSubject,
			SmsPlainText,
			ActionDate
		)
		VALUES(
			@DeviceAlertId,
			@DeviceId,
			@AlertType,
			@Email,
			@MediumType,
			@SentStatus,
			@ErrorMessage,
			@EmailSubject,
			@EmailPlainText,
			@EmailHtmlContent,
			@SmsSubject,
			@SmsPlainText,
			GETUTCDATE()
		)	
END