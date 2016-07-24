/*
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

/**
 * \file      SignalDefiner_Extern.h
 * \brief     Exports API functions for Signal Definer interface
 * \author    Arunkumar Karri
 * \copyright Copyright (c) 2012, Robert Bosch Engineering and Business Solutions.  All rights reserved.
 *
 * Exports API functions for Signal Definer interface
 */

#pragma once
//#include "stdafx.h"
#if defined USAGEMODE
#undef USAGEMODE
#endif

class CGraphList;

#if defined USAGE_EXPORT
#define USAGEMODE   __declspec(dllexport)
#else
#define USAGEMODE   __declspec(dllimport)
#endif

#include "afxwin.h"

#ifdef __cplusplus
extern "C" {  // only need to export C interface if used by C++ source code
#endif

    /*  Exported function list */
    USAGEMODE HRESULT SG_CreateGraphWindow( CMDIFrameWnd* pParentWnd, short eBusType);
    USAGEMODE HRESULT SG_ShowGraphWindow( short eBusType, BOOL bShow = TRUE);
    USAGEMODE BOOL    SG_IsWindowVisible( short eBusType);
    USAGEMODE HRESULT SG_SetRefreshRate( UINT unRefreshRate);
    USAGEMODE HRESULT SG_SetSignalListDetails( short eBusType, CGraphList* pSignalList);
    USAGEMODE HRESULT SG_vPostMessageToSGWnd( short eBusType, UINT msg, WPARAM wParam, LPARAM lParam);

#ifdef __cplusplus
}
#endif
