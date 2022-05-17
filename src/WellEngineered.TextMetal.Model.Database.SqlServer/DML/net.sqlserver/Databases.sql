﻿/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

-- database
-- DECLARE

SELECT
	sys_d.[database_id] AS [DatabaseId],
	sys_d.[name] AS [DatabaseName],
	sys_d.[create_date] AS [CreationTimestamp]
FROM
	[sys].[databases] sys_d
ORDER BY
	sys_d.[name] ASC
