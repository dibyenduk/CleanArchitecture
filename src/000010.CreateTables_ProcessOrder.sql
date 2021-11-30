-- Will change later to run using roundehouse

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tProcessOrder](
	[Id] [int] IDENTITY(1,1) NOT NULL,	
	[Nbr] [varchar](30) NOT NULL,	
	[CreatedBy] [nvarchar](30) NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](30) NOT NULL,
	[ModifiedDateTime] [datetime] NOT NULL,
 CONSTRAINT [tProcessOrder_PK_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY])
GO

ALTER TABLE [dbo].[tProcessOrder] ADD  CONSTRAINT [tProcessOrder_DC_CreatedBy]  DEFAULT (suser_sname()) FOR [CreatedBy]
GO

ALTER TABLE [dbo].[tProcessOrder] ADD  CONSTRAINT [tProcessOrder_DC_CreatedDateTime]  DEFAULT (getutcdate()) FOR [CreatedDateTime]
GO

ALTER TABLE [dbo].[tProcessOrder] ADD  CONSTRAINT [tProcessOrder_DC_ModifiedBy]  DEFAULT (suser_sname()) FOR [ModifiedBy]
GO

ALTER TABLE [dbo].[tProcessOrder] ADD  CONSTRAINT [tProcessOrder_DC_ModifiedDateTime]  DEFAULT (getutcdate()) FOR [ModifiedDateTime]
GO