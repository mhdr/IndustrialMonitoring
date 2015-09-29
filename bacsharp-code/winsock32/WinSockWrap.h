// WinSockWrap.h
// A DLL Wrapper for WinSock

/* COPYRIGHT
 -------------------------------------------
 Copyright (C) 2013 Plus 1 Micro, Inc.

 This program is free software; you can redistribute it and/or
 modify it under the terms of the GNU General Public License
 as published by the Free Software Foundation; either version 2
 of the License, or (at your option) any later version.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with this program; if not, write to:
 The Free Software Foundation, Inc.
 59 Temple Place - Suite 330
 Boston, MA  02111-1307, USA.

 As a special exception, if other files instantiate templates or
 use macros or inline functions from this file, or you compile
 this file and link it with other works to produce a work based
 on this file, this file does not by itself cause the resulting
 work to be covered by the GNU General Public License. However
 the source code for this file must still be made available in
 accordance with section (3) of the GNU General Public License.

 This exception does not invalidate any other reasons why a work
 based on this file might be covered by the GNU General Public
 License.
 -------------------------------------------
*/

extern "C" {
  __declspec(dllexport) int WinSockStartUp();
}

extern "C" {
  __declspec(dllexport) int WinSockSendTo(char* bytes, int count, unsigned long ipaddr);
}

extern "C" {
  __declspec(dllexport) int WinSockRecvReady();
}

extern "C" {
  __declspec(dllexport) int WinSockRecvFrom(char* bytes, int* count, unsigned long* ipaddr);
}

extern "C" {
  __declspec(dllexport) int WinSockShutDown();
}

extern "C" {
  __declspec(dllexport) int WinSockLastError();
}

