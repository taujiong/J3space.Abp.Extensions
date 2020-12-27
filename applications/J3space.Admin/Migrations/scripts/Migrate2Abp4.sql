CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory`
(
    `MigrationId`    varchar(95) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
);

START TRANSACTION;

CREATE TABLE `AbpAuditLogs`
(
    `Id`                   char(36)                           NOT NULL,
    `ApplicationName`      varchar(96) CHARACTER SET utf8mb4  NULL,
    `UserId`               char(36)                           NULL,
    `UserName`             varchar(256) CHARACTER SET utf8mb4 NULL,
    `TenantId`             char(36)                           NULL,
    `TenantName`           longtext CHARACTER SET utf8mb4     NULL,
    `ImpersonatorUserId`   char(36)                           NULL,
    `ImpersonatorTenantId` char(36)                           NULL,
    `ExecutionTime`        datetime(6)                        NOT NULL,
    `ExecutionDuration`    int                                NOT NULL,
    `ClientIpAddress`      varchar(64) CHARACTER SET utf8mb4  NULL,
    `ClientName`           varchar(128) CHARACTER SET utf8mb4 NULL,
    `ClientId`             varchar(64) CHARACTER SET utf8mb4  NULL,
    `CorrelationId`        varchar(64) CHARACTER SET utf8mb4  NULL,
    `BrowserInfo`          varchar(512) CHARACTER SET utf8mb4 NULL,
    `HttpMethod`           varchar(16) CHARACTER SET utf8mb4  NULL,
    `Url`                  varchar(256) CHARACTER SET utf8mb4 NULL,
    `Exceptions`           longtext CHARACTER SET utf8mb4     NULL,
    `Comments`             varchar(256) CHARACTER SET utf8mb4 NULL,
    `HttpStatusCode`       int                                NULL,
    `ExtraProperties`      longtext CHARACTER SET utf8mb4     NULL,
    `ConcurrencyStamp`     varchar(40) CHARACTER SET utf8mb4  NULL,
    CONSTRAINT `PK_AbpAuditLogs` PRIMARY KEY (`Id`)
);

CREATE TABLE `AbpClaimTypes`
(
    `Id`               char(36)                           NOT NULL,
    `Name`             varchar(256) CHARACTER SET utf8mb4 NOT NULL,
    `Required`         tinyint(1)                         NOT NULL,
    `IsStatic`         tinyint(1)                         NOT NULL,
    `Regex`            varchar(512) CHARACTER SET utf8mb4 NULL,
    `RegexDescription` varchar(128) CHARACTER SET utf8mb4 NULL,
    `Description`      varchar(256) CHARACTER SET utf8mb4 NULL,
    `ValueType`        int                                NOT NULL,
    `ExtraProperties`  longtext CHARACTER SET utf8mb4     NULL,
    `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4  NULL,
    CONSTRAINT `PK_AbpClaimTypes` PRIMARY KEY (`Id`)
);

CREATE TABLE `AbpFeatureValues`
(
    `Id`           char(36)                           NOT NULL,
    `Name`         varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `Value`        varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `ProviderName` varchar(64) CHARACTER SET utf8mb4  NULL,
    `ProviderKey`  varchar(64) CHARACTER SET utf8mb4  NULL,
    CONSTRAINT `PK_AbpFeatureValues` PRIMARY KEY (`Id`)
);

CREATE TABLE `AbpLinkUsers`
(
    `Id`             char(36) NOT NULL,
    `SourceUserId`   char(36) NOT NULL,
    `SourceTenantId` char(36) NULL,
    `TargetUserId`   char(36) NOT NULL,
    `TargetTenantId` char(36) NULL,
    CONSTRAINT `PK_AbpLinkUsers` PRIMARY KEY (`Id`)
);

CREATE TABLE `AbpOrganizationUnits`
(
    `Id`                   char(36)                           NOT NULL,
    `TenantId`             char(36)                           NULL,
    `ParentId`             char(36)                           NULL,
    `Code`                 varchar(95) CHARACTER SET utf8mb4  NOT NULL,
    `DisplayName`          varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `ExtraProperties`      longtext CHARACTER SET utf8mb4     NULL,
    `ConcurrencyStamp`     varchar(40) CHARACTER SET utf8mb4  NULL,
    `CreationTime`         datetime(6)                        NOT NULL,
    `CreatorId`            char(36)                           NULL,
    `LastModificationTime` datetime(6)                        NULL,
    `LastModifierId`       char(36)                           NULL,
    `IsDeleted`            tinyint(1)                         NOT NULL DEFAULT FALSE,
    `DeleterId`            char(36)                           NULL,
    `DeletionTime`         datetime(6)                        NULL,
    CONSTRAINT `PK_AbpOrganizationUnits` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AbpOrganizationUnits_AbpOrganizationUnits_ParentId` FOREIGN KEY (`ParentId`) REFERENCES `AbpOrganizationUnits` (`Id`) ON DELETE RESTRICT
);

CREATE TABLE `AbpPermissionGrants`
(
    `Id`           char(36)                           NOT NULL,
    `TenantId`     char(36)                           NULL,
    `Name`         varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `ProviderName` varchar(64) CHARACTER SET utf8mb4  NOT NULL,
    `ProviderKey`  varchar(64) CHARACTER SET utf8mb4  NOT NULL,
    CONSTRAINT `PK_AbpPermissionGrants` PRIMARY KEY (`Id`)
);

CREATE TABLE `AbpRoles`
(
    `Id`               char(36)                           NOT NULL,
    `TenantId`         char(36)                           NULL,
    `Name`             varchar(256) CHARACTER SET utf8mb4 NOT NULL,
    `NormalizedName`   varchar(256) CHARACTER SET utf8mb4 NOT NULL,
    `IsDefault`        tinyint(1)                         NOT NULL,
    `IsStatic`         tinyint(1)                         NOT NULL,
    `IsPublic`         tinyint(1)                         NOT NULL,
    `ExtraProperties`  longtext CHARACTER SET utf8mb4     NULL,
    `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4  NULL,
    CONSTRAINT `PK_AbpRoles` PRIMARY KEY (`Id`)
);

CREATE TABLE `AbpSecurityLogs`
(
    `Id`               char(36)                           NOT NULL,
    `TenantId`         char(36)                           NULL,
    `ApplicationName`  varchar(96) CHARACTER SET utf8mb4  NULL,
    `Identity`         varchar(96) CHARACTER SET utf8mb4  NULL,
    `Action`           varchar(96) CHARACTER SET utf8mb4  NULL,
    `UserId`           char(36)                           NULL,
    `UserName`         varchar(256) CHARACTER SET utf8mb4 NULL,
    `TenantName`       varchar(64) CHARACTER SET utf8mb4  NULL,
    `ClientId`         varchar(64) CHARACTER SET utf8mb4  NULL,
    `CorrelationId`    varchar(64) CHARACTER SET utf8mb4  NULL,
    `ClientIpAddress`  varchar(64) CHARACTER SET utf8mb4  NULL,
    `BrowserInfo`      varchar(512) CHARACTER SET utf8mb4 NULL,
    `CreationTime`     datetime(6)                        NOT NULL,
    `ExtraProperties`  longtext CHARACTER SET utf8mb4     NULL,
    `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4  NULL,
    CONSTRAINT `PK_AbpSecurityLogs` PRIMARY KEY (`Id`)
);

CREATE TABLE `AbpSettings`
(
    `Id`           char(36)                           NOT NULL,
    `Name`         varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `Value`        longtext CHARACTER SET utf8mb4     NOT NULL,
    `ProviderName` varchar(64) CHARACTER SET utf8mb4  NULL,
    `ProviderKey`  varchar(64) CHARACTER SET utf8mb4  NULL,
    CONSTRAINT `PK_AbpSettings` PRIMARY KEY (`Id`)
);

CREATE TABLE `AbpTenants`
(
    `Id`                   char(36)                          NOT NULL,
    `Name`                 varchar(64) CHARACTER SET utf8mb4 NOT NULL,
    `ExtraProperties`      longtext CHARACTER SET utf8mb4    NULL,
    `ConcurrencyStamp`     varchar(40) CHARACTER SET utf8mb4 NULL,
    `CreationTime`         datetime(6)                       NOT NULL,
    `CreatorId`            char(36)                          NULL,
    `LastModificationTime` datetime(6)                       NULL,
    `LastModifierId`       char(36)                          NULL,
    `IsDeleted`            tinyint(1)                        NOT NULL DEFAULT FALSE,
    `DeleterId`            char(36)                          NULL,
    `DeletionTime`         datetime(6)                       NULL,
    CONSTRAINT `PK_AbpTenants` PRIMARY KEY (`Id`)
);

CREATE TABLE `AbpUsers`
(
    `Id`                   char(36)                           NOT NULL,
    `TenantId`             char(36)                           NULL,
    `UserName`             varchar(256) CHARACTER SET utf8mb4 NOT NULL,
    `NormalizedUserName`   varchar(256) CHARACTER SET utf8mb4 NOT NULL,
    `Name`                 varchar(64) CHARACTER SET utf8mb4  NULL,
    `Surname`              varchar(64) CHARACTER SET utf8mb4  NULL,
    `Email`                varchar(256) CHARACTER SET utf8mb4 NOT NULL,
    `NormalizedEmail`      varchar(256) CHARACTER SET utf8mb4 NOT NULL,
    `EmailConfirmed`       tinyint(1)                         NOT NULL DEFAULT FALSE,
    `PasswordHash`         varchar(256) CHARACTER SET utf8mb4 NULL,
    `SecurityStamp`        varchar(256) CHARACTER SET utf8mb4 NOT NULL,
    `IsExternal`           tinyint(1)                         NOT NULL DEFAULT FALSE,
    `PhoneNumber`          varchar(16) CHARACTER SET utf8mb4  NULL,
    `PhoneNumberConfirmed` tinyint(1)                         NOT NULL DEFAULT FALSE,
    `TwoFactorEnabled`     tinyint(1)                         NOT NULL DEFAULT FALSE,
    `LockoutEnd`           datetime(6)                        NULL,
    `LockoutEnabled`       tinyint(1)                         NOT NULL DEFAULT FALSE,
    `AccessFailedCount`    int                                NOT NULL DEFAULT 0,
    `ExtraProperties`      longtext CHARACTER SET utf8mb4     NULL,
    `ConcurrencyStamp`     varchar(40) CHARACTER SET utf8mb4  NULL,
    `CreationTime`         datetime(6)                        NOT NULL,
    `CreatorId`            char(36)                           NULL,
    `LastModificationTime` datetime(6)                        NULL,
    `LastModifierId`       char(36)                           NULL,
    `IsDeleted`            tinyint(1)                         NOT NULL DEFAULT FALSE,
    `DeleterId`            char(36)                           NULL,
    `DeletionTime`         datetime(6)                        NULL,
    CONSTRAINT `PK_AbpUsers` PRIMARY KEY (`Id`)
);

CREATE TABLE `IdentityServerApiResources`
(
    `Id`                                  char(36)                            NOT NULL,
    `Name`                                varchar(200) CHARACTER SET utf8mb4  NOT NULL,
    `DisplayName`                         varchar(200) CHARACTER SET utf8mb4  NULL,
    `Description`                         varchar(1000) CHARACTER SET utf8mb4 NULL,
    `Enabled`                             tinyint(1)                          NOT NULL,
    `AllowedAccessTokenSigningAlgorithms` varchar(100) CHARACTER SET utf8mb4  NULL,
    `ShowInDiscoveryDocument`             tinyint(1)                          NOT NULL,
    `ExtraProperties`                     longtext CHARACTER SET utf8mb4      NULL,
    `ConcurrencyStamp`                    varchar(40) CHARACTER SET utf8mb4   NULL,
    `CreationTime`                        datetime(6)                         NOT NULL,
    `CreatorId`                           char(36)                            NULL,
    `LastModificationTime`                datetime(6)                         NULL,
    `LastModifierId`                      char(36)                            NULL,
    `IsDeleted`                           tinyint(1)                          NOT NULL DEFAULT FALSE,
    `DeleterId`                           char(36)                            NULL,
    `DeletionTime`                        datetime(6)                         NULL,
    CONSTRAINT `PK_IdentityServerApiResources` PRIMARY KEY (`Id`)
);

CREATE TABLE `IdentityServerApiScopes`
(
    `Id`                      char(36)                            NOT NULL,
    `Enabled`                 tinyint(1)                          NOT NULL,
    `Name`                    varchar(200) CHARACTER SET utf8mb4  NOT NULL,
    `DisplayName`             varchar(200) CHARACTER SET utf8mb4  NULL,
    `Description`             varchar(1000) CHARACTER SET utf8mb4 NULL,
    `Required`                tinyint(1)                          NOT NULL,
    `Emphasize`               tinyint(1)                          NOT NULL,
    `ShowInDiscoveryDocument` tinyint(1)                          NOT NULL,
    `ExtraProperties`         longtext CHARACTER SET utf8mb4      NULL,
    `ConcurrencyStamp`        varchar(40) CHARACTER SET utf8mb4   NULL,
    `CreationTime`            datetime(6)                         NOT NULL,
    `CreatorId`               char(36)                            NULL,
    `LastModificationTime`    datetime(6)                         NULL,
    `LastModifierId`          char(36)                            NULL,
    `IsDeleted`               tinyint(1)                          NOT NULL DEFAULT FALSE,
    `DeleterId`               char(36)                            NULL,
    `DeletionTime`            datetime(6)                         NULL,
    CONSTRAINT `PK_IdentityServerApiScopes` PRIMARY KEY (`Id`)
);

CREATE TABLE `IdentityServerClients`
(
    `Id`                                    char(36)                            NOT NULL,
    `ClientId`                              varchar(200) CHARACTER SET utf8mb4  NOT NULL,
    `ClientName`                            varchar(200) CHARACTER SET utf8mb4  NULL,
    `Description`                           varchar(1000) CHARACTER SET utf8mb4 NULL,
    `ClientUri`                             varchar(2000) CHARACTER SET utf8mb4 NULL,
    `LogoUri`                               varchar(2000) CHARACTER SET utf8mb4 NULL,
    `Enabled`                               tinyint(1)                          NOT NULL,
    `ProtocolType`                          varchar(200) CHARACTER SET utf8mb4  NOT NULL,
    `RequireClientSecret`                   tinyint(1)                          NOT NULL,
    `RequireConsent`                        tinyint(1)                          NOT NULL,
    `AllowRememberConsent`                  tinyint(1)                          NOT NULL,
    `AlwaysIncludeUserClaimsInIdToken`      tinyint(1)                          NOT NULL,
    `RequirePkce`                           tinyint(1)                          NOT NULL,
    `AllowPlainTextPkce`                    tinyint(1)                          NOT NULL,
    `RequireRequestObject`                  tinyint(1)                          NOT NULL,
    `AllowAccessTokensViaBrowser`           tinyint(1)                          NOT NULL,
    `FrontChannelLogoutUri`                 varchar(2000) CHARACTER SET utf8mb4 NULL,
    `FrontChannelLogoutSessionRequired`     tinyint(1)                          NOT NULL,
    `BackChannelLogoutUri`                  varchar(2000) CHARACTER SET utf8mb4 NULL,
    `BackChannelLogoutSessionRequired`      tinyint(1)                          NOT NULL,
    `AllowOfflineAccess`                    tinyint(1)                          NOT NULL,
    `IdentityTokenLifetime`                 int                                 NOT NULL,
    `AllowedIdentityTokenSigningAlgorithms` varchar(100) CHARACTER SET utf8mb4  NULL,
    `AccessTokenLifetime`                   int                                 NOT NULL,
    `AuthorizationCodeLifetime`             int                                 NOT NULL,
    `ConsentLifetime`                       int                                 NULL,
    `AbsoluteRefreshTokenLifetime`          int                                 NOT NULL,
    `SlidingRefreshTokenLifetime`           int                                 NOT NULL,
    `RefreshTokenUsage`                     int                                 NOT NULL,
    `UpdateAccessTokenClaimsOnRefresh`      tinyint(1)                          NOT NULL,
    `RefreshTokenExpiration`                int                                 NOT NULL,
    `AccessTokenType`                       int                                 NOT NULL,
    `EnableLocalLogin`                      tinyint(1)                          NOT NULL,
    `IncludeJwtId`                          tinyint(1)                          NOT NULL,
    `AlwaysSendClientClaims`                tinyint(1)                          NOT NULL,
    `ClientClaimsPrefix`                    varchar(200) CHARACTER SET utf8mb4  NULL,
    `PairWiseSubjectSalt`                   varchar(200) CHARACTER SET utf8mb4  NULL,
    `UserSsoLifetime`                       int                                 NULL,
    `UserCodeType`                          varchar(100) CHARACTER SET utf8mb4  NULL,
    `DeviceCodeLifetime`                    int                                 NOT NULL,
    `ExtraProperties`                       longtext CHARACTER SET utf8mb4      NULL,
    `ConcurrencyStamp`                      varchar(40) CHARACTER SET utf8mb4   NULL,
    `CreationTime`                          datetime(6)                         NOT NULL,
    `CreatorId`                             char(36)                            NULL,
    `LastModificationTime`                  datetime(6)                         NULL,
    `LastModifierId`                        char(36)                            NULL,
    `IsDeleted`                             tinyint(1)                          NOT NULL DEFAULT FALSE,
    `DeleterId`                             char(36)                            NULL,
    `DeletionTime`                          datetime(6)                         NULL,
    CONSTRAINT `PK_IdentityServerClients` PRIMARY KEY (`Id`)
);

CREATE TABLE `IdentityServerDeviceFlowCodes`
(
    `Id`               char(36)                           NOT NULL,
    `DeviceCode`       varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `UserCode`         varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `SubjectId`        varchar(200) CHARACTER SET utf8mb4 NULL,
    `SessionId`        varchar(100) CHARACTER SET utf8mb4 NULL,
    `ClientId`         varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `Description`      varchar(200) CHARACTER SET utf8mb4 NULL,
    `Expiration`       datetime(6)                        NOT NULL,
    `Data`             longtext CHARACTER SET utf8mb4     NOT NULL,
    `ExtraProperties`  longtext CHARACTER SET utf8mb4     NULL,
    `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4  NULL,
    `CreationTime`     datetime(6)                        NOT NULL,
    `CreatorId`        char(36)                           NULL,
    CONSTRAINT `PK_IdentityServerDeviceFlowCodes` PRIMARY KEY (`Id`)
);

CREATE TABLE `IdentityServerIdentityResources`
(
    `Id`                      char(36)                            NOT NULL,
    `Name`                    varchar(200) CHARACTER SET utf8mb4  NOT NULL,
    `DisplayName`             varchar(200) CHARACTER SET utf8mb4  NULL,
    `Description`             varchar(1000) CHARACTER SET utf8mb4 NULL,
    `Enabled`                 tinyint(1)                          NOT NULL,
    `Required`                tinyint(1)                          NOT NULL,
    `Emphasize`               tinyint(1)                          NOT NULL,
    `ShowInDiscoveryDocument` tinyint(1)                          NOT NULL,
    `ExtraProperties`         longtext CHARACTER SET utf8mb4      NULL,
    `ConcurrencyStamp`        varchar(40) CHARACTER SET utf8mb4   NULL,
    `CreationTime`            datetime(6)                         NOT NULL,
    `CreatorId`               char(36)                            NULL,
    `LastModificationTime`    datetime(6)                         NULL,
    `LastModifierId`          char(36)                            NULL,
    `IsDeleted`               tinyint(1)                          NOT NULL DEFAULT FALSE,
    `DeleterId`               char(36)                            NULL,
    `DeletionTime`            datetime(6)                         NULL,
    CONSTRAINT `PK_IdentityServerIdentityResources` PRIMARY KEY (`Id`)
);

CREATE TABLE `IdentityServerPersistedGrants`
(
    `Key`              varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `Type`             varchar(50) CHARACTER SET utf8mb4  NOT NULL,
    `SubjectId`        varchar(200) CHARACTER SET utf8mb4 NULL,
    `SessionId`        varchar(100) CHARACTER SET utf8mb4 NULL,
    `ClientId`         varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `Description`      varchar(200) CHARACTER SET utf8mb4 NULL,
    `CreationTime`     datetime(6)                        NOT NULL,
    `Expiration`       datetime(6)                        NULL,
    `ConsumedTime`     datetime(6)                        NULL,
    `Data`             longtext CHARACTER SET utf8mb4     NOT NULL,
    `Id`               char(36)                           NOT NULL,
    `ExtraProperties`  longtext CHARACTER SET utf8mb4     NULL,
    `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4  NULL,
    CONSTRAINT `PK_IdentityServerPersistedGrants` PRIMARY KEY (`Key`)
);

CREATE TABLE `AbpAuditLogActions`
(
    `Id`                char(36)                            NOT NULL,
    `TenantId`          char(36)                            NULL,
    `AuditLogId`        char(36)                            NOT NULL,
    `ServiceName`       varchar(256) CHARACTER SET utf8mb4  NULL,
    `MethodName`        varchar(128) CHARACTER SET utf8mb4  NULL,
    `Parameters`        varchar(2000) CHARACTER SET utf8mb4 NULL,
    `ExecutionTime`     datetime(6)                         NOT NULL,
    `ExecutionDuration` int                                 NOT NULL,
    `ExtraProperties`   longtext CHARACTER SET utf8mb4      NULL,
    CONSTRAINT `PK_AbpAuditLogActions` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AbpAuditLogActions_AbpAuditLogs_AuditLogId` FOREIGN KEY (`AuditLogId`) REFERENCES `AbpAuditLogs` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AbpEntityChanges`
(
    `Id`                 char(36)                           NOT NULL,
    `AuditLogId`         char(36)                           NOT NULL,
    `TenantId`           char(36)                           NULL,
    `ChangeTime`         datetime(6)                        NOT NULL,
    `ChangeType`         tinyint unsigned                   NOT NULL,
    `EntityTenantId`     char(36)                           NULL,
    `EntityId`           varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `EntityTypeFullName` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `ExtraProperties`    longtext CHARACTER SET utf8mb4     NULL,
    CONSTRAINT `PK_AbpEntityChanges` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AbpEntityChanges_AbpAuditLogs_AuditLogId` FOREIGN KEY (`AuditLogId`) REFERENCES `AbpAuditLogs` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AbpOrganizationUnitRoles`
(
    `RoleId`             char(36)    NOT NULL,
    `OrganizationUnitId` char(36)    NOT NULL,
    `TenantId`           char(36)    NULL,
    `CreationTime`       datetime(6) NOT NULL,
    `CreatorId`          char(36)    NULL,
    CONSTRAINT `PK_AbpOrganizationUnitRoles` PRIMARY KEY (`OrganizationUnitId`, `RoleId`),
    CONSTRAINT `FK_AbpOrganizationUnitRoles_AbpOrganizationUnits_OrganizationUn~` FOREIGN KEY (`OrganizationUnitId`) REFERENCES `AbpOrganizationUnits` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AbpOrganizationUnitRoles_AbpRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AbpRoles` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AbpRoleClaims`
(
    `Id`         char(36)                            NOT NULL,
    `RoleId`     char(36)                            NOT NULL,
    `TenantId`   char(36)                            NULL,
    `ClaimType`  varchar(256) CHARACTER SET utf8mb4  NOT NULL,
    `ClaimValue` varchar(1024) CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AbpRoleClaims` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AbpRoleClaims_AbpRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AbpRoles` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AbpTenantConnectionStrings`
(
    `TenantId` char(36)                            NOT NULL,
    `Name`     varchar(64) CHARACTER SET utf8mb4   NOT NULL,
    `Value`    varchar(1024) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_AbpTenantConnectionStrings` PRIMARY KEY (`TenantId`, `Name`),
    CONSTRAINT `FK_AbpTenantConnectionStrings_AbpTenants_TenantId` FOREIGN KEY (`TenantId`) REFERENCES `AbpTenants` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AbpUserClaims`
(
    `Id`         char(36)                            NOT NULL,
    `UserId`     char(36)                            NOT NULL,
    `TenantId`   char(36)                            NULL,
    `ClaimType`  varchar(256) CHARACTER SET utf8mb4  NOT NULL,
    `ClaimValue` varchar(1024) CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AbpUserClaims` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AbpUserClaims_AbpUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AbpUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AbpUserLogins`
(
    `UserId`              char(36)                           NOT NULL,
    `LoginProvider`       varchar(64) CHARACTER SET utf8mb4  NOT NULL,
    `TenantId`            char(36)                           NULL,
    `ProviderKey`         varchar(196) CHARACTER SET utf8mb4 NOT NULL,
    `ProviderDisplayName` varchar(128) CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AbpUserLogins` PRIMARY KEY (`UserId`, `LoginProvider`),
    CONSTRAINT `FK_AbpUserLogins_AbpUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AbpUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AbpUserOrganizationUnits`
(
    `UserId`             char(36)    NOT NULL,
    `OrganizationUnitId` char(36)    NOT NULL,
    `TenantId`           char(36)    NULL,
    `CreationTime`       datetime(6) NOT NULL,
    `CreatorId`          char(36)    NULL,
    CONSTRAINT `PK_AbpUserOrganizationUnits` PRIMARY KEY (`OrganizationUnitId`, `UserId`),
    CONSTRAINT `FK_AbpUserOrganizationUnits_AbpOrganizationUnits_OrganizationUn~` FOREIGN KEY (`OrganizationUnitId`) REFERENCES `AbpOrganizationUnits` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AbpUserOrganizationUnits_AbpUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AbpUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AbpUserRoles`
(
    `UserId`   char(36) NOT NULL,
    `RoleId`   char(36) NOT NULL,
    `TenantId` char(36) NULL,
    CONSTRAINT `PK_AbpUserRoles` PRIMARY KEY (`UserId`, `RoleId`),
    CONSTRAINT `FK_AbpUserRoles_AbpRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AbpRoles` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AbpUserRoles_AbpUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AbpUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AbpUserTokens`
(
    `UserId`        char(36)                           NOT NULL,
    `LoginProvider` varchar(64) CHARACTER SET utf8mb4  NOT NULL,
    `Name`          varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `TenantId`      char(36)                           NULL,
    `Value`         longtext CHARACTER SET utf8mb4     NULL,
    CONSTRAINT `PK_AbpUserTokens` PRIMARY KEY (`UserId`, `LoginProvider`, `Name`),
    CONSTRAINT `FK_AbpUserTokens_AbpUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AbpUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `IdentityServerApiResourceClaims`
(
    `Type`          varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `ApiResourceId` char(36)                           NOT NULL,
    CONSTRAINT `PK_IdentityServerApiResourceClaims` PRIMARY KEY (`ApiResourceId`, `Type`),
    CONSTRAINT `FK_IdentityServerApiResourceClaims_IdentityServerApiResources_A~` FOREIGN KEY (`ApiResourceId`) REFERENCES `IdentityServerApiResources` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `IdentityServerApiResourceProperties`
(
    `ApiResourceId` char(36)                           NOT NULL,
    `Key`           varchar(250) CHARACTER SET utf8mb4 NOT NULL,
    `Value`         varchar(300) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_IdentityServerApiResourceProperties` PRIMARY KEY (`ApiResourceId`, `Key`, `Value`),
    CONSTRAINT `FK_IdentityServerApiResourceProperties_IdentityServerApiResourc~` FOREIGN KEY (`ApiResourceId`) REFERENCES `IdentityServerApiResources` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `IdentityServerApiResourceScopes`
(
    `ApiResourceId` char(36)                           NOT NULL,
    `Scope`         varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_IdentityServerApiResourceScopes` PRIMARY KEY (`ApiResourceId`, `Scope`),
    CONSTRAINT `FK_IdentityServerApiResourceScopes_IdentityServerApiResources_A~` FOREIGN KEY (`ApiResourceId`) REFERENCES `IdentityServerApiResources` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `IdentityServerApiResourceSecrets`
(
    `Type`          varchar(250) CHARACTER SET utf8mb4  NOT NULL,
    `Value`         varchar(300) CHARACTER SET utf8mb4  NOT NULL,
    `ApiResourceId` char(36)                            NOT NULL,
    `Description`   varchar(1000) CHARACTER SET utf8mb4 NULL,
    `Expiration`    datetime(6)                         NULL,
    CONSTRAINT `PK_IdentityServerApiResourceSecrets` PRIMARY KEY (`ApiResourceId`, `Type`, `Value`),
    CONSTRAINT `FK_IdentityServerApiResourceSecrets_IdentityServerApiResources_~` FOREIGN KEY (`ApiResourceId`) REFERENCES `IdentityServerApiResources` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `IdentityServerApiScopeClaims`
(
    `Type`       varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `ApiScopeId` char(36)                           NOT NULL,
    CONSTRAINT `PK_IdentityServerApiScopeClaims` PRIMARY KEY (`ApiScopeId`, `Type`),
    CONSTRAINT `FK_IdentityServerApiScopeClaims_IdentityServerApiScopes_ApiScop~` FOREIGN KEY (`ApiScopeId`) REFERENCES `IdentityServerApiScopes` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `IdentityServerApiScopeProperties`
(
    `ApiScopeId` char(36)                           NOT NULL,
    `Key`        varchar(250) CHARACTER SET utf8mb4 NOT NULL,
    `Value`      varchar(300) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_IdentityServerApiScopeProperties` PRIMARY KEY (`ApiScopeId`, `Key`, `Value`),
    CONSTRAINT `FK_IdentityServerApiScopeProperties_IdentityServerApiScopes_Api~` FOREIGN KEY (`ApiScopeId`) REFERENCES `IdentityServerApiScopes` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `IdentityServerClientClaims`
(
    `ClientId` char(36)                           NOT NULL,
    `Type`     varchar(250) CHARACTER SET utf8mb4 NOT NULL,
    `Value`    varchar(250) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_IdentityServerClientClaims` PRIMARY KEY (`ClientId`, `Type`, `Value`),
    CONSTRAINT `FK_IdentityServerClientClaims_IdentityServerClients_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `IdentityServerClients` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `IdentityServerClientCorsOrigins`
(
    `ClientId` char(36)                           NOT NULL,
    `Origin`   varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_IdentityServerClientCorsOrigins` PRIMARY KEY (`ClientId`, `Origin`),
    CONSTRAINT `FK_IdentityServerClientCorsOrigins_IdentityServerClients_Client~` FOREIGN KEY (`ClientId`) REFERENCES `IdentityServerClients` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `IdentityServerClientGrantTypes`
(
    `ClientId`  char(36)                           NOT NULL,
    `GrantType` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_IdentityServerClientGrantTypes` PRIMARY KEY (`ClientId`, `GrantType`),
    CONSTRAINT `FK_IdentityServerClientGrantTypes_IdentityServerClients_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `IdentityServerClients` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `IdentityServerClientIdPRestrictions`
(
    `ClientId` char(36)                           NOT NULL,
    `Provider` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_IdentityServerClientIdPRestrictions` PRIMARY KEY (`ClientId`, `Provider`),
    CONSTRAINT `FK_IdentityServerClientIdPRestrictions_IdentityServerClients_Cl~` FOREIGN KEY (`ClientId`) REFERENCES `IdentityServerClients` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `IdentityServerClientPostLogoutRedirectUris`
(
    `ClientId`              char(36)                           NOT NULL,
    `PostLogoutRedirectUri` varchar(300) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_IdentityServerClientPostLogoutRedirectUris` PRIMARY KEY (`ClientId`, `PostLogoutRedirectUri`),
    CONSTRAINT `FK_IdentityServerClientPostLogoutRedirectUris_IdentityServerCli~` FOREIGN KEY (`ClientId`) REFERENCES `IdentityServerClients` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `IdentityServerClientProperties`
(
    `ClientId` char(36)                           NOT NULL,
    `Key`      varchar(250) CHARACTER SET utf8mb4 NOT NULL,
    `Value`    varchar(300) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_IdentityServerClientProperties` PRIMARY KEY (`ClientId`, `Key`, `Value`),
    CONSTRAINT `FK_IdentityServerClientProperties_IdentityServerClients_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `IdentityServerClients` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `IdentityServerClientRedirectUris`
(
    `ClientId`    char(36)                           NOT NULL,
    `RedirectUri` varchar(300) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_IdentityServerClientRedirectUris` PRIMARY KEY (`ClientId`, `RedirectUri`),
    CONSTRAINT `FK_IdentityServerClientRedirectUris_IdentityServerClients_Clien~` FOREIGN KEY (`ClientId`) REFERENCES `IdentityServerClients` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `IdentityServerClientScopes`
(
    `ClientId` char(36)                           NOT NULL,
    `Scope`    varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_IdentityServerClientScopes` PRIMARY KEY (`ClientId`, `Scope`),
    CONSTRAINT `FK_IdentityServerClientScopes_IdentityServerClients_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `IdentityServerClients` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `IdentityServerClientSecrets`
(
    `Type`        varchar(250) CHARACTER SET utf8mb4  NOT NULL,
    `Value`       varchar(300) CHARACTER SET utf8mb4  NOT NULL,
    `ClientId`    char(36)                            NOT NULL,
    `Description` varchar(2000) CHARACTER SET utf8mb4 NULL,
    `Expiration`  datetime(6)                         NULL,
    CONSTRAINT `PK_IdentityServerClientSecrets` PRIMARY KEY (`ClientId`, `Type`, `Value`),
    CONSTRAINT `FK_IdentityServerClientSecrets_IdentityServerClients_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `IdentityServerClients` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `IdentityServerIdentityResourceClaims`
(
    `Type`               varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `IdentityResourceId` char(36)                           NOT NULL,
    CONSTRAINT `PK_IdentityServerIdentityResourceClaims` PRIMARY KEY (`IdentityResourceId`, `Type`),
    CONSTRAINT `FK_IdentityServerIdentityResourceClaims_IdentityServerIdentityR~` FOREIGN KEY (`IdentityResourceId`) REFERENCES `IdentityServerIdentityResources` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `IdentityServerIdentityResourceProperties`
(
    `IdentityResourceId` char(36)                           NOT NULL,
    `Key`                varchar(250) CHARACTER SET utf8mb4 NOT NULL,
    `Value`              varchar(300) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_IdentityServerIdentityResourceProperties` PRIMARY KEY (`IdentityResourceId`, `Key`, `Value`),
    CONSTRAINT `FK_IdentityServerIdentityResourceProperties_IdentityServerIdent~` FOREIGN KEY (`IdentityResourceId`) REFERENCES `IdentityServerIdentityResources` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AbpEntityPropertyChanges`
(
    `Id`                   char(36)                           NOT NULL,
    `TenantId`             char(36)                           NULL,
    `EntityChangeId`       char(36)                           NOT NULL,
    `NewValue`             varchar(512) CHARACTER SET utf8mb4 NULL,
    `OriginalValue`        varchar(512) CHARACTER SET utf8mb4 NULL,
    `PropertyName`         varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `PropertyTypeFullName` varchar(64) CHARACTER SET utf8mb4  NOT NULL,
    CONSTRAINT `PK_AbpEntityPropertyChanges` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AbpEntityPropertyChanges_AbpEntityChanges_EntityChangeId` FOREIGN KEY (`EntityChangeId`) REFERENCES `AbpEntityChanges` (`Id`) ON DELETE CASCADE
);

CREATE INDEX `IX_AbpAuditLogActions_AuditLogId` ON `AbpAuditLogActions` (`AuditLogId`);

CREATE INDEX `IX_AbpAuditLogActions_TenantId_ServiceName_MethodName_Execution~` ON `AbpAuditLogActions` (`TenantId`, `ServiceName`, `MethodName`, `ExecutionTime`);

CREATE INDEX `IX_AbpAuditLogs_TenantId_ExecutionTime` ON `AbpAuditLogs` (`TenantId`, `ExecutionTime`);

CREATE INDEX `IX_AbpAuditLogs_TenantId_UserId_ExecutionTime` ON `AbpAuditLogs` (`TenantId`, `UserId`, `ExecutionTime`);

CREATE INDEX `IX_AbpEntityChanges_AuditLogId` ON `AbpEntityChanges` (`AuditLogId`);

CREATE INDEX `IX_AbpEntityChanges_TenantId_EntityTypeFullName_EntityId` ON `AbpEntityChanges` (`TenantId`, `EntityTypeFullName`, `EntityId`);

CREATE INDEX `IX_AbpEntityPropertyChanges_EntityChangeId` ON `AbpEntityPropertyChanges` (`EntityChangeId`);

CREATE INDEX `IX_AbpFeatureValues_Name_ProviderName_ProviderKey` ON `AbpFeatureValues` (`Name`, `ProviderName`, `ProviderKey`);

CREATE UNIQUE INDEX `IX_AbpLinkUsers_SourceUserId_SourceTenantId_TargetUserId_Target~` ON `AbpLinkUsers` (`SourceUserId`,
                                                                                                          `SourceTenantId`,
                                                                                                          `TargetUserId`,
                                                                                                          `TargetTenantId`);

CREATE INDEX `IX_AbpOrganizationUnitRoles_RoleId_OrganizationUnitId` ON `AbpOrganizationUnitRoles` (`RoleId`, `OrganizationUnitId`);

CREATE INDEX `IX_AbpOrganizationUnits_Code` ON `AbpOrganizationUnits` (`Code`);

CREATE INDEX `IX_AbpOrganizationUnits_ParentId` ON `AbpOrganizationUnits` (`ParentId`);

CREATE INDEX `IX_AbpPermissionGrants_Name_ProviderName_ProviderKey` ON `AbpPermissionGrants` (`Name`, `ProviderName`, `ProviderKey`);

CREATE INDEX `IX_AbpRoleClaims_RoleId` ON `AbpRoleClaims` (`RoleId`);

CREATE INDEX `IX_AbpRoles_NormalizedName` ON `AbpRoles` (`NormalizedName`);

CREATE INDEX `IX_AbpSecurityLogs_TenantId_Action` ON `AbpSecurityLogs` (`TenantId`, `Action`);

CREATE INDEX `IX_AbpSecurityLogs_TenantId_ApplicationName` ON `AbpSecurityLogs` (`TenantId`, `ApplicationName`);

CREATE INDEX `IX_AbpSecurityLogs_TenantId_Identity` ON `AbpSecurityLogs` (`TenantId`, `Identity`);

CREATE INDEX `IX_AbpSecurityLogs_TenantId_UserId` ON `AbpSecurityLogs` (`TenantId`, `UserId`);

CREATE INDEX `IX_AbpSettings_Name_ProviderName_ProviderKey` ON `AbpSettings` (`Name`, `ProviderName`, `ProviderKey`);

CREATE INDEX `IX_AbpTenants_Name` ON `AbpTenants` (`Name`);

CREATE INDEX `IX_AbpUserClaims_UserId` ON `AbpUserClaims` (`UserId`);

CREATE INDEX `IX_AbpUserLogins_LoginProvider_ProviderKey` ON `AbpUserLogins` (`LoginProvider`, `ProviderKey`);

CREATE INDEX `IX_AbpUserOrganizationUnits_UserId_OrganizationUnitId` ON `AbpUserOrganizationUnits` (`UserId`, `OrganizationUnitId`);

CREATE INDEX `IX_AbpUserRoles_RoleId_UserId` ON `AbpUserRoles` (`RoleId`, `UserId`);

CREATE INDEX `IX_AbpUsers_Email` ON `AbpUsers` (`Email`);

CREATE INDEX `IX_AbpUsers_NormalizedEmail` ON `AbpUsers` (`NormalizedEmail`);

CREATE INDEX `IX_AbpUsers_NormalizedUserName` ON `AbpUsers` (`NormalizedUserName`);

CREATE INDEX `IX_AbpUsers_UserName` ON `AbpUsers` (`UserName`);

CREATE INDEX `IX_IdentityServerClients_ClientId` ON `IdentityServerClients` (`ClientId`);

CREATE UNIQUE INDEX `IX_IdentityServerDeviceFlowCodes_DeviceCode` ON `IdentityServerDeviceFlowCodes` (`DeviceCode`);

CREATE INDEX `IX_IdentityServerDeviceFlowCodes_Expiration` ON `IdentityServerDeviceFlowCodes` (`Expiration`);

CREATE INDEX `IX_IdentityServerDeviceFlowCodes_UserCode` ON `IdentityServerDeviceFlowCodes` (`UserCode`);

CREATE INDEX `IX_IdentityServerPersistedGrants_Expiration` ON `IdentityServerPersistedGrants` (`Expiration`);

CREATE INDEX `IX_IdentityServerPersistedGrants_SubjectId_ClientId_Type` ON `IdentityServerPersistedGrants` (`SubjectId`, `ClientId`, `Type`);

CREATE INDEX `IX_IdentityServerPersistedGrants_SubjectId_SessionId_Type` ON `IdentityServerPersistedGrants` (`SubjectId`, `SessionId`, `Type`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20201227032029_Migrate2Abp4', '5.0.1');

COMMIT;

