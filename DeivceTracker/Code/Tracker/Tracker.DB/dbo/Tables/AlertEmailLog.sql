CREATE TABLE [dbo].[AlertEmailLog] (
    [DeviceAlertId]    INT             NOT NULL,
    [DeviceId]         NVARCHAR (50)   NULL,
    [AlertType]        INT             NOT NULL,
    [Email]            NVARCHAR (150)  NOT NULL,
    [MediumType]       INT             NULL,
    [SentStatus]       NVARCHAR (50)   NULL,
    [ErrorMessage]     NVARCHAR (1000) NULL,
    [EmailSubject]     TEXT            NULL,
    [EmailPlainText]   TEXT            NULL,
    [EmailHtmlContent] TEXT            NULL,
    [SmsSubject]       TEXT            NULL,
    [SmsPlainText]     TEXT            NULL,
    [ActionDate]       DATETIME        CONSTRAINT [DF_AlertEmailLog_ActionDate] DEFAULT (getutcdate()) NULL
);





