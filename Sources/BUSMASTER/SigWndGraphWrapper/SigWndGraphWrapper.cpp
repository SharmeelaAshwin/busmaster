// SigWndGraphWrapper.cpp : Defines the exported functions for the DLL application.
//


// SigWndWrapper.cpp : Defines the exported functions for the DLL application.
//
#pragma once
#include "SigGraphWndWrapper_stdafx.h"
#include <afxdllx.h>
#include "SigGraphWndWrapper.h"
#include "MsgBufVSE.h"
#include "Utility/Utility_Thread.h"
#include "GraphList.h"
#include "Include/BaseDefs.h"
#include "TimeManager.h"
#include <DataTypes/SigGrphWnd_Datatypes.h>
#include <comutil.h>

#define USAGE_EXPORT
#include "SigWndGraphWrapper_Extern.h"

#import "..\BIN\busmaster_debug\SignalGraphWndCSharp.tlb" named_guids raw_interfaces_only
using namespace SignalGraphWndCSharp;

static AFX_EXTENSION_MODULE SigWndGraphWrapperDLL = { false, nullptr };
ISigGraphWndPtr pISigGraphWnd = nullptr;
ISignalDetailsPtr pISigDetails[MAX_PROTOCOLS];

extern "C" int APIENTRY
DllMain(HINSTANCE hInstance, DWORD dwReason, LPVOID lpReserved)
{
    static HINSTANCE shLangInst=nullptr;

    // Remove this if you use lpReserved
    UNREFERENCED_PARAMETER(lpReserved);

    if (dwReason == DLL_PROCESS_ATTACH)
    {
        TRACE0("SigGraphWndWrapper.DLL Initializing!\n");

        // Extension DLL one-time initialization
        if (!AfxInitExtensionModule(SigWndGraphWrapperDLL, hInstance))
        {
            return 0;
        }

        // Insert this DLL into the resource chain
        // NOTE: If this Extension DLL is being implicitly linked to by
        //  an MFC Regular DLL (such as an ActiveX Control)
        //  instead of an MFC application, then you will want to
        //  remove this line from DllMain and put it in a separate
        //  function exported from this Extension DLL.  The Regular DLL
        //  that uses this Extension DLL should then explicitly call that
        //  function to initialize this Extension DLL.  Otherwise,
        //  the CDynLinkLibrary object will not be attached to the
        //  Regular DLL's resource chain, and serious problems will
        //  result.

        // Begin of Multiple Language support
        //if ( CMultiLanguage::m_nLocales <= 0 )  // Not detected yet
        //{
        //    CMultiLanguage::DetectLangID();     // Detect language as user locale
        //    CMultiLanguage::DetectUILanguage(); // Detect language in MUI OS
        //}
        //TCHAR szModuleFileName[MAX_PATH];       // Get Module File Name and path
        //int ret = ::GetModuleFileName(hInstance, szModuleFileName, MAX_PATH);
        //if ( ret == 0 || ret == MAX_PATH )
        //{
        //    ASSERT(false);
        //}
        //// Load resource-only language DLL. It will use the languages
        //// detected above, take first available language,
        //// or you can specify another language as second parameter to
        //// LoadLangResourceDLL. And try that first.
        //shLangInst = CMultiLanguage::LoadLangResourceDLL( szModuleFileName );
        //if (shLangInst)
        //{
        //    SigWndGraphWrapperDLL.hResource = shLangInst;
        //}
        // End of Multiple Language support

        new CDynLinkLibrary(SigWndGraphWrapperDLL);
		
       /* for(int nBUSID = 0; nBUSID<AVAILABLE_PROTOCOLS; nBUSID++)
        {
            m_sGraphWndPlacement[nBUSID].length = 0;
            m_sGraphWndPlacement[nBUSID].rcNormalPosition.top = -1;
            m_sGraphSplitterPos[nBUSID].m_nRootSplitterData[0][0] = -1;
        }
        g_rcParent.top = -1;*/
    }
    else if (dwReason == DLL_PROCESS_DETACH)
    {
		CoUninitialize();
        if (shLangInst)
        {
            FreeLibrary(shLangInst);
        }

        TRACE0("SigGraphWndWrapper.DLL Terminating!\n");

        // Terminate the library before destructors are called
        AfxTermExtensionModule(SigWndGraphWrapperDLL);
    }
    return 1;   // ok
}

//typedef enum eELEMENT_TYPE
//{
//    eSTAT_PARAM,
//    eRAW_VALUE,
//    ePHY_VALUE
//};
//
//#define MAX_PROTOCOLS				20
//#define AVAILABLE_PROTOCOLS         5
//#define defTIMER_RESOLUTION_FACTOR     10000.0

CMsgBufVSE m_ouMsgInterpretBuffer;
CPARAM_THREADPROC m_ouGraphReadThread;
CGraphList* m_ouGraphList[MAX_PROTOCOLS];

USAGEMODE HRESULT SG_CreateGraphWindow( CMDIFrameWnd* pParentWnd,  short eBusType)
{
	HRESULT hr = CoInitialize(NULL);
	pISigGraphWnd = new ISigGraphWndPtr(__uuidof(SigGraphWnd));
	if(pISigGraphWnd != nullptr)
	{
		__int64  n = 0; 
		pISigGraphWnd->CreateGraphWindow(eBusType, &n);
		/*if (pISigDetails != nullptr && pISigDetails[eBusType] != NULL)
		{
			pISigGraphWnd->SetSignalListDetails(eBusType, pISigDetails[eBusType]);
		}*/
	}

	// Uninitialize COM.
	//CoUninitialize();
	return S_OK;
}

USAGEMODE HRESULT SG_ShowGraphWindow( short eBusType, BOOL bShow)
{
	return S_OK;
}

USAGEMODE BOOL SG_IsWindowVisible( short eBusType)
{
	return 0;
}

BOOL bStartGraphReadThread();
BOOL bStopGraphReadThread();
USAGEMODE HRESULT SG_vPostMessageToSGWnd( short eBusType, UINT msg, WPARAM wParam, LPARAM lParam)
{
	TRY
	{
		if(pISigGraphWnd != nullptr)
		{
			VARIANT_BOOL  bWindowExists = 0;
			if(pISigGraphWnd != NULL)
			{
				pISigGraphWnd->IsGraphWindowCreated(eBusType, &bWindowExists);
			}
			if (bWindowExists == -1)   //-1 is true in VARIANT_BOOL
			{
				//m_pomGraphWindows[eBusType]->PostMessage(msg, wParam, lParam);

				eUSERSELCTION eUserSel = eDATABASEIMPORTCMD;
				eUserSel               = static_cast <eUSERSELCTION>(wParam);
				BOOL bConnect = FALSE;

				switch(eUserSel)
				{
				case eCONNECTCMD:
					bConnect = (BOOL)lParam;
					VARIANT_BOOL retVal;
					pISigGraphWnd->StartStopPlotting(eBusType, bConnect, &retVal);
					if(bConnect)
					{
						if(m_ouGraphReadThread.m_unActionCode != CREATE_TIME_MAP)
						{
							bStartGraphReadThread();
						}
					}
					else
					{
						bStopGraphReadThread();
					}
					break;
				}
			}

		}
	}
	CATCH_ALL(pomException)
	{
		if(pomException != nullptr )
		{
			CHAR scErrorMsg[255];
			// Get the exception error message
			pomException->GetErrorMessage(scErrorMsg,sizeof(scErrorMsg));
			//m_omStrError = scErrorMsg;
			pomException->Delete();
		}
		//bReturn = FALSE;

	}
	END_CATCH_ALL
	

    return S_OK;
}

USAGEMODE CMsgBufVSE* SG_GetGraphBuffer()
{
	return &m_ouMsgInterpretBuffer;
}


/* Function to stop msg read thread*/
BOOL bStopGraphReadThread()
{
    BOOL bReturn = FALSE;
    bReturn = m_ouGraphReadThread.bTerminateThread();
    m_ouGraphReadThread.m_hActionEvent = nullptr;
    m_ouGraphReadThread.m_unActionCode = IDLE;
    return bReturn;
}

USAGEMODE HRESULT SG_SetSignalListDetails( short eBusType, CGraphList* pSignalList)
{
	m_ouGraphList[eBusType] = pSignalList;
	if (pISigGraphWnd != nullptr)
	{
		int signalsCnt = pSignalList->m_omElementList.GetSize();
		pISigDetails[eBusType] = new ISignalDetailsPtr(__uuidof(SignalDetails));
		if(pISigDetails != nullptr)
		{
			pISigDetails[eBusType]->put_NoOfSignals(signalsCnt);
			for (int i = 0; i < signalsCnt; i++)
			{
				pISigDetails[eBusType]->AddSignals(_com_util::ConvertStringToBSTR(pSignalList->m_omElementList[i].m_omStrElementName));
			}
			if (signalsCnt > 0)
			{
				pISigGraphWnd->SetSignalListDetails(eBusType, pISigDetails[eBusType]);
			}	
		}
	}
	return S_OK;
}

/* Read thread function for graph display*/
DWORD WINAPI SignalDataPlotterThread(LPVOID pVoid)
{
    CPARAM_THREADPROC* pThreadParam = (CPARAM_THREADPROC*) pVoid;
    if (pThreadParam == nullptr)
    {
        return (DWORD)-1;
    }

    pThreadParam->m_unActionCode = CREATE_TIME_MAP;
    int nBufferSize = 0;

    /*for(int nBUSId = 0; nBUSId<AVAILABLE_PROTOCOLS; nBUSId++)
    {
        if(m_pomGraphWindows[nBUSId] != nullptr)
        {
            int nRefreshTime =
                m_pomGraphWindows[nBUSId]->m_pGraphList->m_odGraphParameters.m_nRefreshRate;
            nBufferSize = m_pomGraphWindows[nBUSId]->m_pGraphList->m_odGraphParameters.m_nBufferSize;
        }
    }*/

    //Clear the buffer with previous contents if any.
    m_ouMsgInterpretBuffer.vClearMessageBuffer();

    LONGLONG g_nInitTimeStamp[MAX_PROTOCOLS] = {0};
    double g_dElapsdTime[MAX_PROTOCOLS] = {0};
    bool bLoopON = true;

    //Allow the connection related activities take place prior to proceeding with loop
    Sleep(1000);

    UINT64 unPointsCnt = 0;
	
    while (bLoopON)
    {
        //Commented by Arunkumar Karri on 07/02/2012 to make the plotting realtime.
        //Sleep(nRefreshTime);
        //Introduced a 50 Millisecond delay
        Sleep(50);
        if(pThreadParam->m_unActionCode == IDLE)
        {
            bLoopON = false;
        }
        while(m_ouMsgInterpretBuffer.GetMsgCount() > 0)
        {
            INT nType = 0;
            INT nSize = MAX_DATA_LEN_J1939;
            SINTERPRETDATA_LIST sInterpretList;
            sInterpretList.m_unValue.m_dPhysical = 0;
            sInterpretList.m_unValue.m_nRawValue = 0;
            HRESULT hResult = m_ouMsgInterpretBuffer.ReadFromBuffer(nType,
                              (BYTE*)&sInterpretList, nSize);

            //Get access to the Graph control
           /* IDMGraphCtrl* pDMGraphCtrl = m_pomGraphWindows[nType]->m_pDMGraphCtrl;
            if (  pDMGraphCtrl ==nullptr )
            {
                return 0;
            }*/

            //Get the Graph collectrion
            //CComPtr<IDMGraphCollection> spGraphCollection;
            //pDMGraphCtrl->get_Elements(&spGraphCollection);
			VARIANT_BOOL isWindowCreated = false;
			if(pISigGraphWnd != NULL)
			{
				pISigGraphWnd->IsGraphWindowCreated(nType, &isWindowCreated);
			}
			if(hResult == S_OK &&  isWindowCreated == -1)    //-1 is true in VARIANT_BOOL
            {
                if(g_nInitTimeStamp[nType] == 0)
                {
                    g_nInitTimeStamp[nType] = sInterpretList.m_nTimeStamp;

                    g_dElapsdTime[nType] = CTimeManager::nCalculateCurrTimeStamp() -
                                           CTimeManager::nGetAbsoluteTime();
                    g_dElapsdTime[nType] /= defTIMER_RESOLUTION_FACTOR;
                }

                double dAbsTime = (double)sInterpretList.m_nTimeStamp - g_nInitTimeStamp[nType];
                dAbsTime /= defTIMER_RESOLUTION_FACTOR;
                dAbsTime+=g_dElapsdTime[nType];

                CGraphElement odTemp;
                CGraphList* podList = nullptr;
				podList = m_ouGraphList[nType];
                INT_PTR nItemCount  = podList->m_omElementList.GetSize();
                long    lElementCnt = 0;

                for( int nIndex = 0; nIndex < nItemCount; nIndex++ )
                {
                    //CComPtr<IDispatch> spDispatch;
                    //CComPtr<IDMGraphElement> spGraphElement;
                    //if(nIndex >= lElementCnt)       //if new value is added to the graph while transmission then mItemCount will be greater
                    //{
                    //    //then lElementCnt hence the loop will be terminated and the graph will stop
                    //    continue;
                    //}
                   /* hResult = spGraphCollection->get_Item(nIndex, &spDispatch);
                    if ( spDispatch == nullptr )
                    {
                        return 0;
                    }
                    hResult = spDispatch.QueryInterface(&spGraphElement);*/

                    odTemp = podList->m_omElementList[ nIndex ];

                    //if the read data is of type signal
                    if( hResult == S_OK &&
                            odTemp.m_bEnabled == TRUE &&
                            odTemp.m_nValueType != eSTAT_PARAM &&
                            sInterpretList.unMsgID == (unsigned int)odTemp.m_nMsgID)
                    {
                        CString strSigName(sInterpretList.m_acSigName);
                        if(odTemp.m_omStrElementName == strSigName)
                        {
                            if(sInterpretList.m_shType == odTemp.m_nValueType)
                            {
                                if(odTemp.m_nValueType == eRAW_VALUE)
                                {
									pISigGraphWnd->AddGraphPlottingValues(dAbsTime, 
										(double)sInterpretList.m_unValue.m_nRawValue, nType, nIndex);
                                    /*spGraphElement->PlotXY(dAbsTime,
                                                           (double)sInterpretList.m_unValue.m_nRawValue);*/
                                }
                                else
                                {
									pISigGraphWnd->AddGraphPlottingValues(dAbsTime, 
										(double)sInterpretList.m_unValue.m_dPhysical, nType, nIndex);
                                   ///* spGraphElement->PlotXY(dAbsTime,
                                   //                        sInterpretList.m_unValue.m_dPhysical);*/
                                }
                                unPointsCnt++;
                            }
                        }
                    }
                    //If the read data from buffer is of parameter type
                    else if( odTemp.m_bEnabled == TRUE &&
                             odTemp.m_nValueType == eSTAT_PARAM &&
                             sInterpretList.unMsgID == (unsigned int)nIndex)
                    {
                        double dTime = CTimeManager::nCalculateCurrTimeStamp() -
                                       CTimeManager::nGetAbsoluteTime();
                        dTime /= defTIMER_RESOLUTION_FACTOR;
                        if(sInterpretList.m_unValue.m_nRawValue == -1)
                        {
							pISigGraphWnd->AddGraphPlottingValues(dAbsTime, 
										(double)sInterpretList.m_unValue.m_dPhysical, nType, nIndex);
                            /*spGraphElement->PlotXY(dTime,
                                                   sInterpretList.m_unValue.m_dPhysical);*/
                        }
                        else
                        {
							pISigGraphWnd->AddGraphPlottingValues(dAbsTime, 
										(double)sInterpretList.m_unValue.m_nRawValue, nType, nIndex);
                            /*spGraphElement->PlotXY(dTime,
                                                   (double)sInterpretList.m_unValue.m_nRawValue);*/
                        }
                        unPointsCnt++;
                    }
                }
                //If the plotted points exceeds the maximum buffer limit
                if ( unPointsCnt > 1000000 )
                {
					//TODO: Call clear graph here.
                    //vClearGraphElementPoints(spGraphCollection);
                    //After clearing the Graph, sleep for 100 msec
                    Sleep(100);
                    unPointsCnt = 0;
                }
            }
        }
        double dAbsTime = CTimeManager::nCalculateCurrTimeStamp() -
                          CTimeManager::nGetAbsoluteTime();
        dAbsTime /= defTIMER_RESOLUTION_FACTOR;

        for(register int nBusID = 0; nBusID<AVAILABLE_PROTOCOLS; nBusID++)
        {
            //if(m_pomGraphWindows[nBusID] == nullptr)
            //{
            //    continue;
            //}

            ////check if windows is already distroyed.
            //if(!IsWindow(m_pomGraphWindows[nBusID]->m_hWnd))
            //{
            //    m_pomGraphWindows[nBusID]->m_pDMGraphCtrl = nullptr;
            //    m_pomGraphWindows[nBusID] = nullptr;
            //    continue;
            //}

            //if ( m_pomGraphWindows[nBusID]->m_pDMGraphCtrl == nullptr )
            //{
            //    break;
            //}

            ////IDMGraphCtrl* pDMGraphCtrl = m_pomGraphWindows[nBusID]->m_pDMGraphCtrl;

            //if ( pDMGraphCtrl == nullptr )
            //{
            //    break;
            //}

            ////Update X-axis Range
            //double dXMin = 0.0, dXMax = 0.0,dYMin = 0.0, dYMax = 0.0;
            //// Get Present X,Y Axis Values
            //pDMGraphCtrl->GetRange(&dXMin, &dXMax,&dYMin, &dYMax);
            //// Check the Max Range is lesser then the present time value
            //// If the time axis is old then set the latest value
            //if( dXMax < dAbsTime )
            //{
            //    double dRange = dXMax - dXMin;
            //    // Update Graph Control
            //    pDMGraphCtrl->SetRange(dAbsTime - dRange, dAbsTime, dYMin, dYMax);
            //}
        }
    }
    return 0;
}

/* Function to start Msg read thread*/
BOOL bStartGraphReadThread()
{
	BOOL bReturn = FALSE;
    //First stop the thread if running
    bStopGraphReadThread();
    m_ouGraphReadThread.m_hActionEvent = nullptr;
    m_ouGraphReadThread.m_unActionCode = IDLE;
    m_ouGraphReadThread.m_pBuffer = nullptr;
    m_ouGraphReadThread.m_hActionEvent = m_ouMsgInterpretBuffer.hGetNotifyingEvent();
    bReturn = m_ouGraphReadThread.bStartThread(SignalDataPlotterThread);
    return bReturn;
}