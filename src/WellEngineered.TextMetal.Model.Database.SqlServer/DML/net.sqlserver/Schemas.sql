﻿/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

-- schemas
SELECT
	sys_s.[schema_id] AS [SchemaId],
	sys_s.[principal_id] AS [OwnerId],
    sys_s.name AS [SchemaName]
FROM
    [sys].[schemas] sys_s
ORDER BY
	sys_s.[name] ASC
