CREATE TABLE [dbo].[DeviceAlerts] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (150) NULL,
    [DescriptionText] NVARCHAR (150) NULL,
    [DeviceId]        NVARCHAR (50)  NULL,
    [AlertType]       INT            NULL,
    [Eval]            NVARCHAR (150) NULL,
    [Operand]         NVARCHAR (50)  NULL,
    [Operator]        NVARCHAR (50)  NULL,
    [Value]           NVARCHAR (50)  NULL,
    [IsActive]        BIT            CONSTRAINT [DF__DeviceAle__IsAct__4CA06362] DEFAULT ((1)) NULL,
    [CreatedDate]     DATETIME       CONSTRAINT [DF__DeviceAle__Creat__4D94879B] DEFAULT (getutcdate()) NULL,
    [ModifiedDate]    DATETIME       CONSTRAINT [DF__DeviceAle__Modif__4E88ABD4] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK__DeviceAl__3214EC074AB81AF0] PRIMARY KEY CLUSTERED ([Id] ASC)
);





