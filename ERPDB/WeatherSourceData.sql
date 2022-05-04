CREATE TABLE [dbo].[WeatherSourceData](
	[ttimen] [datetime] NOT NULL,
	[ttime] [char](14) NOT NULL,
	[resource_id] [varchar](14) NOT NULL,
	[geocode] [nvarchar](10) NOT NULL,
	[pop12h] [int] NULL,
	[Wx] [nvarchar](50) NULL,
	[Wx_Code] [int] NULL,
	[AT] [int] NULL,
	[T] [int] NULL,
	[RH] [int] NULL,
	[CI] [int] NULL,
	[WeatherDescription] [nvarchar](100) NULL,
	[pop6h] [int] NULL,
	[WS] [nvarchar](10) NULL,
	[WD] [nvarchar](20) NULL,
	[Td] [int] NULL,
 CONSTRAINT [PK_WeatherSourceData] PRIMARY KEY CLUSTERED 
(
	[ttime] ASC,
	[resource_id] ASC,
	[geocode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]