CREATE PROCEDURE [dbo].[A_GetCustomerDetail]
(
	@AlertMediumType INT = NULL,
	@DeviceId NVARCHAR(50) = NULL
)
AS
BEGIN

	SELECT
		'' UserId,
		'' Username,
		'' FirstName,
		'' LastName,
		'' Address_AddressLine1,
		'' Address_AddressLine2,
		'' Address_AddressLine3,
		'' Address_City,
		'' Address_State,
		'' Address_Country,
		'' Address_PostalCode,
		'' PhoneNo,
		'' Email,
		'' WebSite,
		'' CompanyName,
		'' Status,
		'' Parent,
		'' Discriminator,
		
		500 SMSBalance,
		500 EMAILBalance,
		500 NOTIFICATIONBalance,
		
		CAST(0 AS BIT) IsAccountExpired		
		
END