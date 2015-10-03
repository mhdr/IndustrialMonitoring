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

#include "WinSockWrap.h"
#include <winsock2.h>

#pragma comment(lib, "ws2_32.lib") // Winsock Library

#define BUFLEN 512   // Max length of buffer
#define PORT 47808   // The port on which to listen for incoming data

static SOCKET sock;
static struct sockaddr_in server;
static struct sockaddr_in di_other;
static struct sockaddr_in si_other;

//static int slen, recv_len;
static char buf[BUFLEN];
static WSADATA wsa;

extern int WinSockStartUp()
{
  // Initialise winsock
  if (WSAStartup(MAKEWORD(2, 2), &wsa) != 0) return -1;
     
  // Create a socket
  if ((sock = socket(AF_INET, SOCK_DGRAM, 0)) == INVALID_SOCKET) return -2;
     
  // Prepare the sockaddr_in structure
  server.sin_family = AF_INET;
  server.sin_addr.s_addr = htonl(INADDR_ANY);
  server.sin_port = htons(PORT);
     
  // Bind
  if (bind(sock, (struct sockaddr *)&server, sizeof(server)) == SOCKET_ERROR) return -3;

  return 1;
}

extern int WinSockSendTo(char* bytes, int count, unsigned long ipaddr)
{
  // Enable Broadcast
  BOOL opt = TRUE;
  int optlen = sizeof(BOOL);
  int r = setsockopt(sock, SOL_SOCKET, SO_BROADCAST, (char*)&opt, optlen);
  if (r == SOCKET_ERROR) return -1;

  // Prepare the sockaddr_in structure
  di_other.sin_family = AF_INET;
  //di_other.sin_addr.s_addr = inet_addr("192.168.92.255");
  //di_other.sin_addr.s_addr = htonl(INADDR_BROADCAST);
  di_other.sin_addr.s_addr = htonl(ipaddr);
  di_other.sin_port = htons(PORT);
  int slen = sizeof(di_other);

  //if (sendto(sock, buf, count, 0, (struct sockaddr*) &di_other, slen) == SOCKET_ERROR) return -2;
  if (sendto(sock, bytes, count, 0, (struct sockaddr*) &di_other, slen) == SOCKET_ERROR) return -2;
  return 1;
}

extern int WinSockRecvReady()
{
  // Setup fd_set structure
  fd_set fds;
  FD_ZERO (&fds);
  FD_SET (sock, &fds);

  // Setup the timeout
  timeval timeout;
  timeout.tv_sec = 0;   // 0,0 return immediately
  timeout.tv_usec = 0;

  // Return value:
  // -1: error occurred
  // 0: timed out
  // > 0: data ready to be read
  return select(0, &fds, 0, 0, &timeout);
}

extern int WinSockRecvFrom(char* bytes, int* count, unsigned long* ipaddr)
{
  int slen = sizeof(si_other);
  //if ((rlen = recvfrom(sock, buf, BUFLEN, 0, (struct sockaddr *) &si_other, &slen)) == SOCKET_ERROR) return -1;
  if ((*count = recvfrom(sock, bytes, BUFLEN, 0, (struct sockaddr *) &si_other, &slen)) == SOCKET_ERROR) return -1;
  *ipaddr = ntohl(si_other.sin_addr.s_addr);
  return 1;
}

extern int WinSockShutDown()
{
  if (closesocket(sock) == SOCKET_ERROR) return -1;
  if (WSACleanup() == SOCKET_ERROR) return -2;
  return 1;
}

extern int WinSockLastError()
{
  return WSAGetLastError();
}
