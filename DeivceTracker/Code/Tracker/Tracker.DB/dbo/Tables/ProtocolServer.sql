CREATE TABLE [dbo].[ProtocolServer] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [ProtocolServer]   NVARCHAR (150) NULL,
    [Port]             INT            NULL,
    [DevicesConnected] INT            NULL,
    [Action]           NVARCHAR (50)  NULL,
    [ActionText]       NVARCHAR (MAX) NULL,
    [ActionTime]       DATETIME       CONSTRAINT [DF_ProtocolServer_ActionTime] DEFAULT (getutcdate()) NULL,
    [CreatedDate]      DATETIME       CONSTRAINT [DF_ProtocolServer_CreatedDate] DEFAULT (getutcdate()) NULL,
    [ModifiedDate]     DATETIME       CONSTRAINT [DF_ProtocolServer_ModifiedDate] DEFAULT (getutcdate()) NULL,
    [CreatedBy]        NVARCHAR (150) NULL,
    [ModifiedBy]       NVARCHAR (150) NULL,
    CONSTRAINT [PK_ProtocolServer] PRIMARY KEY CLUSTERED ([Id] ASC)
);



