USE [master]
GO
/****** Object:  Database [TestEY_2]    Script Date: 7/9/2015 4:31:29 PM ******/
CREATE DATABASE [TestEY_2] ON  PRIMARY 
( NAME = N'TestEY_2', FILENAME = N'C:\SqlServer\Data\TestEY_2.mdf' , SIZE = 488448KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'TestEY_2_log', FILENAME = N'L:\SqlServerL\Log\TestEY_2_log.ldf' , SIZE = 1475904KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [TestEY_2] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TestEY_2].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TestEY_2] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TestEY_2] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TestEY_2] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TestEY_2] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TestEY_2] SET ARITHABORT OFF 
GO
ALTER DATABASE [TestEY_2] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [TestEY_2] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TestEY_2] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TestEY_2] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TestEY_2] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TestEY_2] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TestEY_2] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TestEY_2] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TestEY_2] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TestEY_2] SET  DISABLE_BROKER 
GO
ALTER DATABASE [TestEY_2] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TestEY_2] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TestEY_2] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TestEY_2] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TestEY_2] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TestEY_2] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [TestEY_2] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TestEY_2] SET RECOVERY FULL 
GO
ALTER DATABASE [TestEY_2] SET  MULTI_USER 
GO
ALTER DATABASE [TestEY_2] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TestEY_2] SET DB_CHAINING OFF 
GO
EXEC sys.sp_db_vardecimal_storage_format N'TestEY_2', N'ON'
GO
USE [TestEY_2]
GO
/****** Object:  FullTextCatalog [Seach]    Script Date: 7/9/2015 4:31:29 PM ******/
CREATE FULLTEXT CATALOG [Seach]WITH ACCENT_SENSITIVITY = ON

GO
/****** Object:  UserDefinedTableType [dbo].[Filter]    Script Date: 7/9/2015 4:31:29 PM ******/
CREATE TYPE [dbo].[Filter] AS TABLE(
	[FieldName] [nvarchar](max) NULL,
	[ValueFirst] [nvarchar](max) NULL,
	[ValueLast] [nvarchar](max) NULL
)
GO
/****** Object:  Table [dbo].[Brands]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Brands](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StoreId] [int] NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[State] [bit] NULL,
	[Ordering] [int] NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Brands] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Categories]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StoreId] [int] NULL,
	[ParentId] [int] NULL,
	[Ordering] [int] NULL CONSTRAINT [DF_Categories_Ordering]  DEFAULT ((1)),
	[CategoryType] [nvarchar](255) NULL,
	[Name] [nvarchar](255) NULL,
	[State] [bit] NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Comments]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [int] NULL,
	[StoreId] [int] NULL,
	[Name] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NULL,
	[CommentType] [nvarchar](255) NULL,
	[Comment] [nvarchar](max) NULL,
	[State] [bit] NULL,
	[Ordering] [int] NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Companies]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Companies](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Client] [nvarchar](255) NULL,
	[Sector] [nvarchar](255) NULL,
	[Address1] [nvarchar](255) NULL,
	[Address2] [nvarchar](255) NULL,
	[PostalCode] [nvarchar](255) NULL,
	[District] [nvarchar](255) NULL,
	[City] [nvarchar](255) NULL,
	[Country] [nvarchar](255) NULL,
	[Tel1] [nvarchar](255) NULL,
	[Tel2] [nvarchar](255) NULL,
	[Tel3] [nvarchar](255) NULL,
	[Fax1] [nvarchar](255) NULL,
	[Fax2] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NULL,
	[Web] [nvarchar](255) NULL,
 CONSTRAINT [PK_Memo$] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Contacts]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contacts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StoreId] [int] NULL,
	[Name] [nvarchar](255) NULL,
	[Title] [nvarchar](200) NULL,
	[Email] [nvarchar](300) NULL,
	[PhoneWork] [nvarchar](200) NULL,
	[PhoneCell] [nvarchar](200) NULL,
	[Ordering] [int] NULL,
	[State] [bit] NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ContentFiles]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContentFiles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ContentId] [int] NULL,
	[FileManagerId] [int] NULL,
 CONSTRAINT [PK_ProductFiles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Contents]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contents](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StoreId] [int] NULL,
	[CategoryId] [int] NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Type] [nvarchar](50) NULL,
	[MainPage] [bit] NULL,
	[State] [bit] NULL,
	[Ordering] [int] NULL,
	[CreatedDate] [datetime2](7) NULL,
	[ImageState] [bit] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EmailLists]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailLists](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StoreId] [int] NULL,
	[FirstName] [nvarchar](255) NULL,
	[LastName] [nvarchar](255) NULL,
	[Email] [nvarchar](500) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[State] [bit] NULL,
	[Ordering] [int] NULL,
 CONSTRAINT [PK_EmailLists] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FileManagers]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileManagers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StoreId] [int] NULL,
	[GoogleImageId] [nvarchar](500) NULL,
	[OriginalFilename] [nvarchar](500) NULL,
	[Title] [nvarchar](500) NULL,
	[ContentType] [nvarchar](255) NULL,
	[ContentLength] [int] NULL,
	[State] [bit] NOT NULL,
	[Ordering] [int] NULL,
	[ThumbnailLink] [nvarchar](500) NULL,
	[IconLink] [nvarchar](500) NULL,
	[WebContentLink] [nvarchar](500) NULL,
	[ModifiedDate] [datetime] NULL,
	[CreatedDate] [datetime2](7) NULL,
	[IsCarousel] [bit] NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[Width] [int] NULL,
	[Height] [int] NULL,
	[FileStatus] [nvarchar](50) NULL,
 CONSTRAINT [PK_Media] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LabelLines]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LabelLines](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ItemId] [int] NULL,
	[ItemType] [nvarchar](50) NULL,
	[LabelId] [int] NULL,
 CONSTRAINT [PK_LabelLines] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_LabelLines] UNIQUE NONCLUSTERED 
(
	[ItemId] ASC,
	[ItemType] ASC,
	[LabelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Labels]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Labels](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StoreId] [int] NULL,
	[ParentId] [int] NULL,
	[Name] [nvarchar](255) NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[Ordering] [int] NULL,
	[State] [bit] NULL,
 CONSTRAINT [PK_Labels] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Locations]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Locations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StoreId] [int] NULL,
	[Address] [nvarchar](500) NULL,
	[City] [nvarchar](200) NULL,
	[LocationState] [nvarchar](200) NULL,
	[Postal] [nvarchar](200) NULL,
	[Country] [nvarchar](200) NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
	[Ordering] [int] NULL,
	[State] [bit] NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Locations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Navigations]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Navigations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StoreId] [int] NULL,
	[ParentId] [int] NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Modul] [nvarchar](255) NULL,
	[ActionName] [nvarchar](255) NULL,
	[ControllerName] [nvarchar](255) NULL,
	[Static] [bit] NULL,
	[Ordering] [int] NULL,
	[Link] [nvarchar](500) NULL,
	[LinkState] [bit] NULL,
	[CreatedDate] [datetime2](7) NULL,
	[State] [bit] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Navigation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PageDesigns]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PageDesigns](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](255) NULL,
	[PageRazorTemplate] [nvarchar](max) NULL,
	[StoreId] [int] NULL,
	[State] [bit] NULL,
	[Ordering] [int] NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_PageDesign] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ProductCategories]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductCategories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StoreId] [int] NULL,
	[ParentId] [int] NULL,
	[Ordering] [int] NULL CONSTRAINT [DF_ProductCategories_Ordering]  DEFAULT ((1)),
	[CategoryType] [nvarchar](255) NULL,
	[Name] [nvarchar](255) NULL,
	[State] [bit] NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_ProductCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ProductFiles]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductFiles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NULL,
	[FileManagerId] [int] NULL,
	[IsMainImage] [bit] NULL,
 CONSTRAINT [PK_ProductFiles_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Products]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StoreId] [int] NULL,
	[ProductCategoryId] [int] NOT NULL,
	[BrandId] [int] NULL,
	[ProductCode] [nvarchar](50) NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Type] [nvarchar](50) NULL,
	[MainPage] [bit] NULL,
	[State] [bit] NULL,
	[Ordering] [int] NULL,
	[CreatedDate] [datetime2](7) NULL,
	[ImageState] [bit] NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[Price] [float] NULL,
	[Discount] [float] NULL,
 CONSTRAINT [PK_Products_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Settings]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Settings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StoreId] [int] NULL,
	[Name] [nvarchar](255) NULL,
	[Description] [nvarchar](max) NULL,
	[SettingKey] [nvarchar](255) NULL,
	[SettingValue] [nvarchar](max) NULL,
	[State] [bit] NULL,
	[Type] [nvarchar](255) NULL,
	[Ordering] [int] NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Config] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Stores]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Stores](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CategoryId] [int] NULL,
	[Name] [nvarchar](255) NULL,
	[Domain] [nvarchar](255) NULL,
	[Layout] [nvarchar](255) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[Ordering] [int] NULL,
	[State] [bit] NULL,
	[GoogleDriveClientId] [nvarchar](500) NULL,
	[GoogleDriveUserEmail] [nvarchar](500) NULL,
	[GoogleDriveUserEmailPassword] [nvarchar](500) NULL,
	[GoogleDriveFolder] [nvarchar](500) NULL,
	[GoogleDriveServiceAccountEmail] [nvarchar](500) NULL,
	[GoogleDriveCertificateP12FileName] [nvarchar](500) NULL,
	[GoogleDrivePassword] [nvarchar](500) NULL,
	[GoogleDriveCertificateP12RawData] [varbinary](max) NULL,
 CONSTRAINT [PK_Stores] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[StoreUsers]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StoreUsers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StoreId] [int] NULL,
	[UserId] [int] NULL,
	[State] [bit] NULL,
	[Ordering] [int] NULL CONSTRAINT [DF_StoreUsers_Ordering]  DEFAULT ((1)),
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_StoreUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[system_logging]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[system_logging](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[system_logging_guid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_system_logging_system_logging_guid]  DEFAULT (newid()),
	[entered_date] [datetime] NULL CONSTRAINT [DF_system_logging_entered_date]  DEFAULT (getdate()),
	[log_application] [varchar](200) NULL,
	[log_date] [varchar](100) NULL,
	[log_level] [varchar](100) NULL,
	[log_logger] [varchar](8000) NULL,
	[log_message] [varchar](8000) NULL,
	[log_machine_name] [varchar](8000) NULL,
	[log_user_name] [varchar](8000) NULL,
	[log_call_site] [varchar](8000) NULL,
	[log_thread] [varchar](100) NULL,
	[log_exception] [varchar](8000) NULL,
	[log_stacktrace] [varchar](8000) NULL,
 CONSTRAINT [PK_system_logging] PRIMARY KEY CLUSTERED 
(
	[system_logging_guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserProfile]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfile](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](56) NOT NULL,
	[FirstName] [nvarchar](255) NULL,
	[LastName] [nvarchar](255) NULL,
	[PhoneNumber] [nvarchar](255) NULL,
	[CreatedDate] [datetime] NULL,
	[LastLoginDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[webpages_Membership]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_Membership](
	[UserId] [int] NOT NULL,
	[CreateDate] [datetime] NULL,
	[ConfirmationToken] [nvarchar](128) NULL,
	[IsConfirmed] [bit] NULL DEFAULT ((0)),
	[LastPasswordFailureDate] [datetime] NULL,
	[PasswordFailuresSinceLastSuccess] [int] NOT NULL DEFAULT ((0)),
	[Password] [nvarchar](128) NOT NULL,
	[PasswordChangedDate] [datetime] NULL,
	[PasswordSalt] [nvarchar](128) NOT NULL,
	[PasswordVerificationToken] [nvarchar](128) NULL,
	[PasswordVerificationTokenExpirationDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[webpages_OAuthMembership]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_OAuthMembership](
	[Provider] [nvarchar](30) NOT NULL,
	[ProviderUserId] [nvarchar](100) NOT NULL,
	[UserId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Provider] ASC,
	[ProviderUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[webpages_Roles]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[webpages_UsersInRoles]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_UsersInRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Index [IX_Categories]    Script Date: 7/9/2015 4:31:29 PM ******/
CREATE NONCLUSTERED INDEX [IX_Categories] ON [dbo].[Categories]
(
	[StoreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Contacts]    Script Date: 7/9/2015 4:31:29 PM ******/
CREATE NONCLUSTERED INDEX [IX_Contacts] ON [dbo].[Contacts]
(
	[StoreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Contents]    Script Date: 7/9/2015 4:31:29 PM ******/
CREATE NONCLUSTERED INDEX [IX_Contents] ON [dbo].[Contents]
(
	[StoreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FileManagers]    Script Date: 7/9/2015 4:31:29 PM ******/
CREATE NONCLUSTERED INDEX [IX_FileManagers] ON [dbo].[FileManagers]
(
	[StoreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Labels]    Script Date: 7/9/2015 4:31:29 PM ******/
CREATE NONCLUSTERED INDEX [IX_Labels] ON [dbo].[Labels]
(
	[StoreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Navigations]    Script Date: 7/9/2015 4:31:29 PM ******/
CREATE NONCLUSTERED INDEX [IX_Navigations] ON [dbo].[Navigations]
(
	[StoreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_PageDesigns]    Script Date: 7/9/2015 4:31:29 PM ******/
CREATE NONCLUSTERED INDEX [IX_PageDesigns] ON [dbo].[PageDesigns]
(
	[StoreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductCategories]    Script Date: 7/9/2015 4:31:29 PM ******/
CREATE NONCLUSTERED INDEX [IX_ProductCategories] ON [dbo].[ProductCategories]
(
	[StoreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Products]    Script Date: 7/9/2015 4:31:29 PM ******/
CREATE NONCLUSTERED INDEX [IX_Products] ON [dbo].[Products]
(
	[StoreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Settings]    Script Date: 7/9/2015 4:31:29 PM ******/
CREATE NONCLUSTERED INDEX [IX_Settings] ON [dbo].[Settings]
(
	[StoreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_StoreUsers]    Script Date: 7/9/2015 4:31:29 PM ******/
CREATE NONCLUSTERED INDEX [IX_StoreUsers] ON [dbo].[StoreUsers]
(
	[StoreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Comments] ADD  CONSTRAINT [DF_Comments_Ordering]  DEFAULT ((1)) FOR [Ordering]
GO
ALTER TABLE [dbo].[webpages_UsersInRoles]  WITH CHECK ADD  CONSTRAINT [fk_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[webpages_Roles] ([RoleId])
GO
ALTER TABLE [dbo].[webpages_UsersInRoles] CHECK CONSTRAINT [fk_RoleId]
GO
ALTER TABLE [dbo].[webpages_UsersInRoles]  WITH CHECK ADD  CONSTRAINT [fk_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserProfile] ([UserId])
GO
ALTER TABLE [dbo].[webpages_UsersInRoles] CHECK CONSTRAINT [fk_UserId]
GO
/****** Object:  StoredProcedure [dbo].[DeleteStore]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--  [DeleteStore] @StoreId=15
CREATE PROCEDURE [dbo].[DeleteStore]
	@StoreId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	 DELETE FROM [dbo].[Settings] where StoreId=@StoreId
	 DELETE FROM [dbo].[Categories] where StoreId=@StoreId
	 DELETE FROM [dbo].[Contents] where StoreId=@StoreId
     DELETE FROM [dbo].[ContentFiles] where [FileManagerId] IN ( SELECT  Id FROM [dbo].[FileManagers] where StoreId=@StoreId)
	 DELETE FROM [dbo].[FileManagers] where StoreId=@StoreId
     DELETE FROM [dbo].[Navigations] where StoreId=@StoreId
	 DELETE FROM [dbo].[PageDesigns]  where StoreId=@StoreId
	 DELETE FROM [dbo].[EmailLists] where StoreId=@StoreId


	 DELETE FROM [dbo].[ProductFiles] where [FileManagerId] IN ( SELECT  Id FROM [dbo].[FileManagers] where StoreId=@StoreId)
	 DELETE FROM [dbo].[ProductCategories] where StoreId=@StoreId
	 DELETE FROM [dbo].[Products] where StoreId=@StoreId
	 DELETE FROM [dbo].[Brands] where StoreId=@StoreId

	DELETE FROM [dbo].[Comments] where StoreId=@StoreId
	DELETE FROM [dbo].[Locations] where StoreId=@StoreId
	DELETE FROM [dbo].[Contacts] where StoreId=@StoreId

	  DELETE FROM [dbo].[LabelLines] where LabelId IN ( SELECT  Id FROM [dbo].[Labels] where StoreId=@StoreId)
	 DELETE FROM [dbo].[Labels] where Id=@StoreId
	 update [dbo].[UserProfile] set FirstName='deleted', LastName='deleted'
	 where [UserId] IN ( SELECT  Id FROM [dbo].[StoreUsers] where StoreId=@StoreId) 


    DELETE FROM [dbo].[StoreUsers] where StoreId=@StoreId
	    DELETE FROM [dbo].[Stores] where Id=@StoreId


END

GO
/****** Object:  StoredProcedure [dbo].[DuplicateStoreData]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec [DuplicateStoreData] @StoreId=5,@StoreName='faqulte.com',@Domain='faqulte.com',@Layout='Layout1'
-- exec [DuplicateStoreData] @StoreId=5,@StoreName='login.seatechnologyjobs.com',@Domain='login.seatechnologyjobs.com',@Layout='Layout1'
-- exec [DuplicateStoreData] @StoreId=5,@StoreName='demirane.com',@Domain='demirane.com',@Layout='Layout1'
-- exec [DuplicateStoreData] @StoreId=5,@StoreName='saatfotografcisi.net',@Domain='saatfotografcisi.net',@Layout='Layout1'
CREATE PROCEDURE [dbo].[DuplicateStoreData]
	-- Add the parameters for the stored procedure here
	@StoreId int,
	@StoreName nvarchar(255),
	@Domain  nvarchar(255) ,
	@Layout  nvarchar(255) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	 
	select * INTO #tmpStore from [dbo].[Stores] where Id=@StoreId

	INSERT INTO [dbo].[Stores] ([CategoryId]
           ,[Name]
           ,[Domain]
           ,[Layout]
           ,[CreatedDate]
           ,[UpdatedDate]
           ,[Ordering]
           ,[State]
           ,[GoogleDriveClientId]
           ,[GoogleDriveUserEmail]
           ,[GoogleDriveFolder]
           ,[GoogleDriveServiceAccountEmail]
           ,[GoogleDriveCertificateP12FileName]
           ,[GoogleDrivePassword])  
	SELECT [CategoryId]
           ,@StoreName
           ,@Domain
           ,@Layout
           ,[CreatedDate]
           ,[UpdatedDate]
           ,[Ordering]
           ,[State]
           ,[GoogleDriveClientId]
           ,[GoogleDriveUserEmail]
           ,[GoogleDriveFolder]
           ,[GoogleDriveServiceAccountEmail]
           ,[GoogleDriveCertificateP12FileName]
           ,[GoogleDrivePassword] from #tmpStore
	declare @NewStoreId int 
    SET @NewStoreId = SCOPE_IDENTITY()


	
 

	INSERT INTO [dbo].[Navigations]
           ([StoreId]
           ,[ParentId]
           ,[Name]
           ,[Modul]
           ,[ControllerName]
           ,[Static]
           ,[Ordering]
           ,[Link]
           ,[LinkState]
           ,[CreatedDate]
           ,[State])
    SELECT @NewStoreId,[ParentId],[Name],[Modul],[ControllerName],[Static],[Ordering],[Link],[LinkState],[CreatedDate],[State] FROM [dbo].[Navigations] WHERE StoreId=@StoreId


	
	DECLARE @OutputContents TABLE (ID INT)


	INSERT INTO [dbo].[Contents]
           ([StoreId]
           ,[CategoryId]
           ,[Name]
           ,[Description]
           ,[Type]
           ,[MainPage]
           ,[State]
           ,[Ordering]
           ,[CreatedDate]
           ,[ImageState])
		   OUTPUT INSERTED.ID INTO @OutputContents(ID)
	SELECT @NewStoreId,[CategoryId],[Name],[Description],[Type],[MainPage],[State],[Ordering],[CreatedDate],[ImageState] FROM [dbo].[Contents] WHERE StoreId=@StoreId

	DECLARE @OutputFileManagers TABLE (ID INT)

	INSERT INTO [dbo].[FileManagers]
           ([StoreId]
           ,[GoogleImageId]
           ,[OriginalFilename]
           ,[Title]
           ,[ContentType]
           ,[ContentLength]
           ,[State]
           ,[Ordering]
           ,[ThumbnailLink]
           ,[IconLink]
           ,[WebContentLink]
           ,[ModifiedDate]
           ,[CreatedDate]
           ,[IsCarousel])
		   OUTPUT INSERTED.ID INTO @OutputFileManagers(ID)
	SELECT  @NewStoreId
      ,[GoogleImageId]
      ,[OriginalFilename]
      ,[Title]
      ,[ContentType]
      ,[ContentLength]
      ,[State]
      ,[Ordering]
      ,[ThumbnailLink]
      ,[IconLink]
      ,[WebContentLink]
      ,[ModifiedDate]
      ,[CreatedDate]
      ,[IsCarousel]
  FROM  [dbo].[FileManagers] WHERE StoreId=@StoreId   AND GoogleImageId is NOT null



		   
  Declare @Keys Table (ID integer Primary Key Not Null)
	Insert @Keys(ID)
	SELECT  ID
	  FROM @OutputFileManagers
    
    Declare @Key Integer
     While Exists (Select * From @Keys)
     Begin
         Select @Key = Max(ID) From @Keys
 
     INSERT INTO [dbo].[ContentFiles]
           ([ContentId]
           ,[FileManagerId])
 		   select top 5 percent @Key,ID from @OutputFileManagers order by newid()
		    
        -- print @Key
         Delete @Keys Where ID = @Key
     End 
 



  	DECLARE @OutputCategories TABLE (ID INT)
		INSERT INTO [dbo].[Categories]
           ([StoreId]
           ,[ParentId]
           ,[Ordering]
           ,[CategoryType]
           ,[Name]
           ,[State]
           ,[CreatedDate])
		    OUTPUT INSERTED.ID INTO @OutputCategories(ID)
			  SELECT @NewStoreId
				  ,[ParentId]
				  ,[Ordering]
				  ,[CategoryType]
				  ,[Name]
				  ,[State]
				  ,[CreatedDate]
			  FROM [dbo].[Categories] WHERE StoreId=@StoreId

			   
 
	Insert @Keys(ID)
	SELECT  Id
	  FROM [dbo].[Contents]
    WHERE StoreId=@NewStoreId  and [Type]='product'

    declare @TempCatId INT=0
     While Exists (Select * From @Keys)
     Begin
         Select @Key = Max(ID) From @Keys
		
		SELECT TOP 1 @TempCatId=ID FROM [dbo].[Categories] where  StoreId=@NewStoreId and [CategoryType]='product'  ORDER BY newid()
        Update [dbo].[Contents] SET CategoryId=@TempCatId WHERE Id=@Key 
		    
        print @Key
         Delete @Keys Where ID = @Key
     End 




  INSERT INTO [dbo].[Settings]
           ([StoreId]
           ,[Name]
           ,[Description]
           ,[SettingKey]
           ,[SettingValue]
           ,[State]
           ,[Type]
           ,[Ordering])
 SELECT  @NewStoreId
      ,[Name]
      ,[Description]
      ,[SettingKey]
      ,[SettingValue]
      ,[State]
      ,[Type]
      ,[Ordering]
  FROM [dbo].[Settings] WHERE StoreId=@StoreId



  INSERT INTO [dbo].[Locations]
           ([StoreId]
           ,[Address]
           ,[City]
           ,[LocationState]
           ,[Postal]
           ,[Country]
           ,[Latitude]
           ,[Longitude]
           ,[Ordering]
           ,[State]
           ,[CreatedDate]
           ,[UpdatedDate])
SELECT  @NewStoreId
      ,[Address]
      ,[City]
      ,[LocationState]
      ,[Postal]
      ,[Country]
      ,[Latitude]
      ,[Longitude]
      ,[Ordering]
      ,[State]
      ,[CreatedDate]
      ,[UpdatedDate]
  FROM [dbo].[Locations]  WHERE StoreId=@StoreId



  INSERT INTO [dbo].[Contacts]
           ([StoreId]
           ,[Name]
           ,[Title]
           ,[Email]
           ,[PhoneWork]
           ,[PhoneCell]
           ,[Ordering]
           ,[State]
           ,[CreatedDate]
           ,[UpdatedDate])
  SELECT  @NewStoreId
      ,[Name]
      ,[Title]
      ,[Email]
      ,[PhoneWork]
      ,[PhoneCell]
      ,[Ordering]
      ,[State]
      ,[CreatedDate]
      ,[UpdatedDate]
  FROM [dbo].[Contacts] WHERE StoreId=@StoreId


  INSERT INTO [dbo].[Products]
           ([StoreId]
           ,[ProductCategoryId]
           ,[Name]
           ,[Description]
           ,[Type]
           ,[MainPage]
           ,[State]
           ,[Ordering]
           ,[CreatedDate]
           ,[ImageState]
           ,[UpdatedDate])
		   SELECT  @NewStoreId
			  ,[ProductCategoryId]
			  ,[Name]
			  ,[Description]
			  ,[Type]
			  ,[MainPage]
			  ,[State]
			  ,[Ordering]
			  ,[CreatedDate]
			  ,[ImageState]
			  ,[UpdatedDate]
		FROM [dbo].[Products] WHERE StoreId=@StoreId


		INSERT INTO [dbo].[ProductCategories]
           ([StoreId]
           ,[ParentId]
           ,[Ordering]
           ,[CategoryType]
           ,[Name]
           ,[State]
           ,[CreatedDate]
           ,[UpdatedDate])
	   SELECT   @NewStoreId
			  ,[ParentId]
			  ,[Ordering]
			  ,[CategoryType]
			  ,[Name]
			  ,[State]
			  ,[CreatedDate]
			  ,[UpdatedDate]
			FROM [dbo].[ProductCategories] WHERE StoreId=@StoreId


			INSERT INTO [dbo].[Labels]
           ([StoreId]
           ,[ParentId]
           ,[Name]
           ,[UpdatedDate]
           ,[CreatedDate]
           ,[Ordering]
           ,[State])
		   SELECT  @NewStoreId
				  ,[ParentId]
				  ,[Name]
				  ,[UpdatedDate]
				  ,[CreatedDate]
				  ,[Ordering]
				  ,[State]
			FROM [dbo].[Labels]   WHERE StoreId=@StoreId


			
		INSERT INTO [dbo].[EmailLists]
				   ([StoreId]
				   ,[FirstName]
				   ,[LastName]
				   ,[Email]
				   ,[CreatedDate]
				   ,[UpdatedDate]
				   ,[State]
				   ,[Ordering])
			SELECT @NewStoreId
				  ,[FirstName]
				  ,[LastName]
				  ,[Email]
				  ,[CreatedDate]
				  ,[UpdatedDate]
				  ,[State]
				  ,[Ordering]
		FROM [dbo].[EmailLists] WHERE StoreId=@StoreId


 
		INSERT INTO [dbo].[Comments]
				   ([ParentId]
				   ,[StoreId]
				   ,[Name]
				   ,[Email]
				   ,[CommentType]
				   ,[Comment]
				   ,[State]
				   ,[Ordering]
				   ,[CreatedDate]
				   ,[UpdatedDate])
		SELECT  [ParentId]
			  ,@NewStoreId
			  ,[Name]
			  ,[Email]
			  ,[CommentType]
			  ,[Comment]
			  ,[State]
			  ,[Ordering]
			  ,[CreatedDate]
			  ,[UpdatedDate]
		  FROM [dbo].[Comments]  WHERE StoreId=@StoreId


INSERT INTO [dbo].[Brands]
           ([StoreId]
           ,[Name]
           ,[Description]
           ,[State]
           ,[Ordering]
           ,[CreatedDate]
           ,[UpdatedDate])
   SELECT @NewStoreId
      ,[Name]
      ,[Description]
      ,[State]
      ,[Ordering]
      ,[CreatedDate]
      ,[UpdatedDate]
  FROM [dbo].[Brands]
 WHERE StoreId=@StoreId

   
END

GO
/****** Object:  StoredProcedure [dbo].[log_DeleteApplicationLogs]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[log_DeleteApplicationLogs]
	 @AppName nvarchar(250) = NULL,
	 @LastDay int = 3
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	  
  	 delete from [dbo].[system_logging] where system_logging_guid IN (
  				SELECT system_logging_guid  
				FROM [dbo].[system_logging]
  				WHERE  DATEDIFF(Day, log_date, getdate() ) >= @LastDay and 
				log_application = ISNULL(@AppName,log_application))
   
  	 
  	 
END

GO
/****** Object:  StoredProcedure [dbo].[log_GetApplicationLogs]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 /*
 
log.lm_sp_GetApplicationLogs 'Content Gathering','Info', 100,90

*/
CREATE PROCEDURE [dbo].[log_GetApplicationLogs]
	 @AppName nvarchar(250) = NULL,
	 @logLevel nvarchar(100) = NULL,
	 @top int= 20,
	 @skip int = 0,
	 @search  nvarchar(100) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	
	DECLARE @logLevels TABLE (Name nvarchar(100) , Ord int)
	INSERT INTO @logLevels (Name,Ord)
	VALUES ('Off',10)
	
	INSERT INTO @logLevels (Name,Ord)
	VALUES ('Fatal',20)
	
	INSERT INTO @logLevels (Name,Ord)
	VALUES ('Error',30)
	
	INSERT INTO @logLevels (Name,Ord)
	VALUES ('Warn',40)
	
		
	INSERT INTO @logLevels (Name,Ord)
	VALUES ('Info',50)
	
	INSERT INTO @logLevels (Name,Ord)
	VALUES ('Debug',60)
	
		INSERT INTO @logLevels (Name,Ord)
	VALUES ('Trace',70)
 
 
	SELECT  
	       system_logging_guid, ROW_NUMBER() OVER (ORDER BY entered_date desc) AS RowNumber INTO #ret
		FROM  [dbo].[system_logging]
		where log_application LIKE  '%' + ISNULL(@AppName, log_application)+ '%'  
		 and log_message LIKE  '%' + ISNULL(@search, log_message)+ '%' 
		
		AND log_level
		IN
		(
		
		 SELECT Name FROM @logLevels
		 WHERE Ord<=
		 ISNULL(
		 (
		 SELECT Ord FROM @logLevels
		 WHERE Name=@logLevel
		 ), 1)
		
		)
		
	
   select *  INTO #ret2 from  #ret where RowNumber BETWEEN @skip+1 AND @skip+@top
      
 
		  
  
  	 SELECT  
			  s.[entered_date]
			  ,s.[log_application]
			  ,s.[log_date]
			  ,s.[log_level]
			  ,s.[log_logger]
			  ,s.[log_message]
			  ,s.[log_call_site]
			  ,s.[log_exception]
			  ,s.[log_stacktrace]
       FROM  [dbo].[system_logging] s inner join #ret2 r 
		ON s.system_logging_guid=r.system_logging_guid
			DECLARE @RC INT
		SET @RC=@@RowCount
		
		 SELECT COUNT(*) RecordsTotal, @skip+1 recordFirst,  @skip+@top recordLast, @RC recordCount
		FROM #ret
	 
		 
  	 drop table #ret
  	 drop table #ret2 
  	 
  	 
END

GO
/****** Object:  StoredProcedure [dbo].[log_GetApplicationNames]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE   PROCEDURE [dbo].[log_GetApplicationNames]
	 
AS
BEGIN
 SELECT act.Log_application, LastActivity,LastError   FROM
(

SELECT Log_application, MAX(log_date) as LastActivity FROM
[dbo].[system_logging]
GROUP BY Log_application
) act

LEFT OUTER JOIN
(


SELECT Log_application, MAX(log_date) as LastError
FROM [dbo].[system_logging]
WHERE log_level IN ('Off', 'Fatal', 'Error')
GROUP BY Log_application
) err
ON act.Log_application=err.Log_application

  	 
  	 
END

GO
/****** Object:  StoredProcedure [dbo].[SearchCompanies]    Script Date: 7/9/2015 4:31:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
/*

DECLARE @filter AS dbo.[Filter]
 
 --- INSERT INTO @filter (FieldName, ValueFirst, ValueLast) VALUES('sektor',N'OTOMOTIV','')

--INSERT INTO @filter (FieldName, ValueFirst, ValueLast) VALUES('country',N'OTOMOTİV','')

--INSERT INTO @filter (FieldName, ValueFirst, ValueLast) VALUES('Brand','foruno','')

--INSERT INTO @filter (FieldName, ValueFirst, ValueLast) VALUES('state','New York','')

--INSERT INTO @filter (FieldName, ValueFirst, ValueLast) VALUES('city','Tuzla-Istanbul','')
 

exec [dbo].[SearchCompanies]
@search = 'ÖZDOĞANLAR', 
@top = 20, 
@skip = 0, 
@filter= @filter
 
 SELECT Sector
FROM [dbo].[Companies]
group by sector



*/
CREATE PROCEDURE [dbo].[SearchCompanies]
	@search nvarchar(200)='',
	@top int=20,
	@skip int=0,
	@filter AS dbo.[Filter] Readonly 
AS
BEGIN

	SET NOCOUNT ON;
	
	/*
IF EXISTS(select name from tempdb..sysobjects  where name like '#recordFound%') drop table #recordFound
*/
CREATE TABLE #recordFound (CompanyId int, FiltersMatched int)


INSERT INTO #recordFound (CompanyId, FiltersMatched)
SELECT Id, 0
FROM [dbo].[Companies]

 

DECLARE @FieldName nvarchar(100)
DECLARE @ValueFirst nvarchar(100)

DECLARE cFilters CURSOR FOR  
SELECT FieldName, ValueFirst
FROM @filter


OPEN cFilters  
FETCH NEXT FROM cFilters INTO @FieldName,  @ValueFirst 


Declare @companyType nvarchar(255)
Declare @brand nvarchar(255)
Declare @country nvarchar(255)
Declare @state nvarchar(255)
Declare @city nvarchar(255)	
	 
set @companyType = NULL 
set @brand = NULL 
set @country = NULL 
set @state = NULL 
set @city = NULL 
 
		
WHILE @@FETCH_STATUS = 0  
BEGIN  

	print @ValueFirst
	
	
	 
	IF @FieldName='Sektor'
	BEGIN
		UPDATE rf
			SET FiltersMatched=FiltersMatched+1
		FROM 
		 #recordFound rf 
		 INNER JOIN 
		 (
			SELECT DISTINCT Id
			FROM  [dbo].[Companies]
			WHERE Sector=@ValueFirst

		  ) m
		  ON rf.CompanyId = m.Id
		  
	 
	 
	END
	
 	IF @FieldName='City'
	BEGIN
		UPDATE rf
			SET FiltersMatched=FiltersMatched+1
		FROM 
		 #recordFound rf 
		 INNER JOIN 
		 (
			SELECT DISTINCT Id
			FROM  [dbo].[Companies]
			WHERE City=@ValueFirst

		  ) m
		  ON rf.CompanyId = m.Id
		  
		  set @country = @ValueFirst 
		 
	END
	
	IF @FieldName='Ulke'
	BEGIN
		UPDATE rf
			SET FiltersMatched=FiltersMatched+1
		FROM 
		 #recordFound rf 
		 INNER JOIN 
		 (
			SELECT DISTINCT Id
			FROM  [dbo].[Companies]
			WHERE Country=@ValueFirst

		  ) m
		  ON rf.CompanyId = m.Id
		  
		  set @country = @ValueFirst 
		 
	END
	
		
	IF @FieldName='Semt'
	BEGIN
		UPDATE rf
			SET FiltersMatched=FiltersMatched+1
		FROM 
		 #recordFound rf 
		 INNER JOIN 
		 (
			SELECT DISTINCT Id
			FROM  [dbo].[Companies]
			WHERE [District]=@ValueFirst

		  ) m
		  ON rf.CompanyId = m.Id
		  
		  set @country = @ValueFirst 
		 
	END
	  
	
	FETCH NEXT FROM cFilters INTO @FieldName,  @ValueFirst 

END  

CLOSE cFilters  
DEALLOCATE cFilters 

--select * from #recordFound WHERE FiltersMatched>0

DELETE FROM #recordFound WHERE FiltersMatched<(SELECT COUNT(*) FROM @filter)

/*
IF EXISTS(select name from tempdb..sysobjects  where name like '#recordFound2%') drop table #recordFound
*/

CREATE TABLE #recordFound2 (CompanyId int, rowNumber int)


		
				
				
		IF (LEN (ISNULL(@search,''))>0)  
		BEGIN
				
				INSERT INTO  #recordFound2 (CompanyId , rowNumber )
				SELECT s.Id as CompanyId, ROW_NUMBER() OVER (ORDER BY [RANK] DESC)  AS rowNumber 
				FROM  [dbo].[Companies] s   
				INNER JOIN 	FREETEXTTABLE ([dbo].[Companies], *, @search) ft ON s.Id=ft.[Key]
				INNER JOIN  #recordFound rf ON rf.CompanyId=s.Id

			 
 
		END
		ELSE
		BEGIN
			
			INSERT INTO  #recordFound2 (CompanyId , rowNumber )
			SELECT c.Id, 	ROW_NUMBER() OVER (ORDER BY c.Id) AS rowNumber 
				FROM #recordFound rf
				INNER join [dbo].[Companies] c
					ON rf.CompanyId=c.Id
			order by rowNumber
		
		END
		
		
		
	    SELECT c.*
			 FROM #recordFound2 rf
				INNER join [dbo].[Companies] c ON rf.CompanyId=c.Id
			WHERE rf.rowNumber BETWEEN @skip+1 AND @skip+@top
		
		DECLARE @RC INT
		SET @RC=@@RowCount
				 

		SELECT * 
			FROM
			(	

			 
				SELECT top 10  'Sektor' as FieldName,CAST(ct.Sector as nvarchar(50))   as ValueFirst,'' as ValueLast, Count(*) cnt, 10 ord   
				FROM       [dbo].[Companies] ct
				INNER JOIN #recordFound2 rf on rf.CompanyId=ct.Id
				group by ct.Sector
				HAVING ct.Sector <>''
				
					UNION
				 
				SELECT top 10  'Ulke' as FieldName,CAST(ct.country as nvarchar(50))   as ValueFirst,'' as ValueLast,Count(*) cnt, 20 ord   
				FROM       [dbo].[Companies] ct
				INNER JOIN #recordFound2 rf on rf.CompanyId=ct.Id
				group by ct.country
				HAVING ct.country <>''
				
					UNION
					
				SELECT top 10 'Semt' as FieldName,CAST(ct.[District] as nvarchar(50))   as ValueFirst,'' as ValueLast,Count(*) cnt, 30 ord   
				FROM     [dbo].[Companies] ct
				INNER JOIN #recordFound2 rf on rf.CompanyId=ct.Id
				group by ct.[District]	
				HAVING ct.[District] <>''
			
				 
					UNION
					
				SELECT top 10 'City' as FieldName,CAST(ct.City as nvarchar(50))   as ValueFirst,'' as ValueLast,Count(*) cnt, 40 ord   
				FROM     [dbo].[Companies] ct
				INNER JOIN #recordFound2 rf on rf.CompanyId=ct.Id
				group by ct.City	
				HAVING ct.City <>''
			
				 
			 
			 
				
				--select * from dbo.med_v_LocationActive  where CompanyId = 107178
				
 ) AS E ORDER BY ord

				
		SELECT COUNT(*) RecordsTotal, @skip+1 recordFirst,  @skip+@top recordLast, @RC recordCount
		FROM #recordFound2 

	 
		



 
END


GO
USE [master]
GO
ALTER DATABASE [TestEY_2] SET  READ_WRITE 
GO
