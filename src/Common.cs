/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace WellEngineered.TextMetal
{
#if !ASYNC_ALL_THE_WAY_DOWN && ASYNC_MAIN_ENTRY_POINT
	#error ERROR
#endif
}