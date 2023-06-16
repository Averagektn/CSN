#define _CRT_SECURE_NO_WARNINGS
#pragma comment(lib, "mpr.lib")
#pragma comment(lib, "netapi32.lib")

#include <winsock2.h>
#include <iptypes.h>
#include <iphlpapi.h>
#include <windows.h>
#include <stdio.h>
#include <winnetwk.h>

void DisplayStruct(int i, LPNETRESOURCE lpnrLocal);

typedef struct _ASTAT_
{
	ADAPTER_STATUS adapt;
	NAME_BUFFER NameBuff[30];
} ASTAT, * PASTAT;
ASTAT Adapter;


BOOL GetMacAddress()
{
	char buffer[50] = { 0 };
	NCB ncb;
	UCHAR uRetCode;
	LANA_ENUM lenum;
	memset(&ncb, 0, sizeof(ncb));
	ncb.ncb_command = NCBENUM;
	ncb.ncb_length = sizeof(lenum);
	ncb.ncb_buffer = (PUCHAR)&lenum;
	uRetCode = Netbios(&ncb);
	for (int i = 0; i < lenum.length; i++)
	{
		memset(&ncb, 0, sizeof(ncb));
		ncb.ncb_command = NCBRESET;
		ncb.ncb_lana_num = lenum.lana[i];
		uRetCode = Netbios(&ncb);
		memset(&ncb, 0, sizeof(ncb));
		ncb.ncb_command = NCBASTAT;
		ncb.ncb_lana_num = lenum.lana[i];
		strcpy((char*)ncb.ncb_callname, "* ");
		ncb.ncb_buffer = (unsigned char*)&Adapter;
		ncb.ncb_length = sizeof(Adapter);
		uRetCode = Netbios(&ncb);
		if (uRetCode == 0)
		{
			sprintf(buffer, "%02X-%02X-%02X-%02X-%02X-%02X\n",
				Adapter.adapt.adapter_address[0],
				Adapter.adapt.adapter_address[1],
				Adapter.adapt.adapter_address[2],
				Adapter.adapt.adapter_address[3],
				Adapter.adapt.adapter_address[4],
				Adapter.adapt.adapter_address[5]);
			printf(buffer);
		}
	}
	return FALSE;
}

BOOL WINAPI EnumerateFunc(LPNETRESOURCE lpnr) {
	DWORD dwResult, dwResultEnum;
	HANDLE hEnum; 
	DWORD cbBuffer = 16384; 
	DWORD cEntries = -1; 
	LPNETRESOURCE lpnrLocal;
	DWORD i;
	dwResult = WNetOpenEnum(RESOURCE_GLOBALNET, RESOURCETYPE_ANY, 0, lpnr,&hEnum); 
	if (dwResult != NO_ERROR) {
		printf("WnetOpenEnum failed with error %d\n", (int)dwResult);
		return FALSE;
	}
	lpnrLocal = (LPNETRESOURCE)GlobalAlloc(GPTR, cbBuffer); 
	if (lpnrLocal == NULL) {
		printf("WnetOpenEnum failed with error %d\n", (int)dwResult);
		return FALSE;
	}

	do {
		ZeroMemory(lpnrLocal, cbBuffer);
		dwResultEnum = WNetEnumResource(hEnum,&cEntries,lpnrLocal,&cbBuffer);
		if (dwResultEnum == NO_ERROR) {
			for (i = 0; i < cEntries; i++) {
				DisplayStruct(i, &lpnrLocal[i]);
				if (RESOURCEUSAGE_CONTAINER == (lpnrLocal[i].dwUsage & RESOURCEUSAGE_CONTAINER))
					if (!EnumerateFunc(&lpnrLocal[i]))
						printf("EnumerateFunc returned FALSE\n");
			}
		}
		else if (dwResultEnum != ERROR_NO_MORE_ITEMS) {
			printf("WNetEnumResource failed with error %d\n", (int)dwResultEnum);
			break;
		}
	} while (dwResultEnum != ERROR_NO_MORE_ITEMS);

	GlobalFree((HGLOBAL)lpnrLocal);
	dwResult = WNetCloseEnum(hEnum);
	if (dwResult != NO_ERROR) {
		printf("WNetCloseEnum failed with error %d\n", (int)dwResult);
		return FALSE;
	}
	return TRUE;
}

void DisplayStruct(int i, LPNETRESOURCE lpnrLocal) // расшифровка
{
	printf("NETRESOURCE[%d] Scope: ", i);
	switch (lpnrLocal->dwScope) {
	case (RESOURCE_CONNECTED):
		printf("connected\n");
		break;
	case (RESOURCE_GLOBALNET):
		printf("all resources\n");
		break;
	case (RESOURCE_REMEMBERED):
		printf("remembered\n");
		break;
	default:
		printf("unknown scope %d\n", (int)lpnrLocal->dwScope);
		break;
	}

	printf("NETRESOURCE[%d] Type: ", i);
	switch (lpnrLocal->dwType) {
	case (RESOURCETYPE_ANY):
		printf("any\n");
		break;
	case (RESOURCETYPE_DISK):
		printf("disk\n");
		break;
	case (RESOURCETYPE_PRINT):
		printf("print\n");
		break;
	default:
		printf("unknown type %d\n", (int)lpnrLocal->dwType);
		break;
	}

	printf("NETRESOURCE[%d] DisplayType: ", i);
	switch (lpnrLocal->dwDisplayType) {
	case (RESOURCEDISPLAYTYPE_GENERIC):
		printf("generic\n");
		break;
	case (RESOURCEDISPLAYTYPE_DOMAIN):
		printf("domain\n");
		break;
	case (RESOURCEDISPLAYTYPE_SERVER):
		printf("server\n");
		break;
	case (RESOURCEDISPLAYTYPE_SHARE):
		printf("share\n");
		break;
	case (RESOURCEDISPLAYTYPE_FILE):
		printf("file\n");
		break;
	case (RESOURCEDISPLAYTYPE_GROUP):
		printf("group\n");
		break;
	case (RESOURCEDISPLAYTYPE_NETWORK):
		printf("network\n");
		break;
	default:
		printf("unknown display type %d\n", (int)lpnrLocal->dwDisplayType);
		break;
	}

	printf("NETRESOURCE[%d] Usage: 0x%x = ", i, (int)lpnrLocal->dwUsage);
	if (lpnrLocal->dwUsage & RESOURCEUSAGE_CONNECTABLE)
		printf("connectable ");
	if (lpnrLocal->dwUsage & RESOURCEUSAGE_CONTAINER)
		printf("container ");
	printf("\n");

	printf("NETRESOURCE[%d] Localname: %S\n", i, (wchar_t*)lpnrLocal->lpLocalName);
	printf("NETRESOURCE[%d] Remotename: %S\n", i, (wchar_t*)lpnrLocal->lpRemoteName);
	printf("NETRESOURCE[%d] Comment: %S\n", i, (wchar_t*)lpnrLocal->lpComment);
	printf("NETRESOURCE[%d] Provider: %S\n", i, (wchar_t*)lpnrLocal->lpProvider);
	printf("\n");
}

int main() {
	printf("Getting the MAC Address: \n");
	GetMacAddress();

	printf("\nEnumerating Network Resources: \n");
	LPNETRESOURCE lpnr = NULL;

	if (EnumerateFunc(lpnr) == FALSE)
		printf("Call to EnumerateFunc failed\n");

	system("pause");
}
