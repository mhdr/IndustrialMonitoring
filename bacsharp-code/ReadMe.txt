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

This is a work in progress!  It is intented to provide a simple and easy C# interface
to BACnet on a Windows platform.  Right off the bat, you may notice that this code uses
DLL calls to the WinSock C library.  This is because the .NET (version 3.5) Socket class 
does not allow send and receive instances to be open to the same port at the same time. You
must Open a Send instance, close it, and then open a Receive instance to get the response. 
It works, but the results are unsatisfactory. (Maybe in newer versions of .NET this will 
be fixed?)  An alternative is to use WinSock, which provides a lower level access to UDP.  

Contents:
	WinSockWrap - The DLL (C++/C) code
		(Note - this is unmanaged code, you must copy the DLL to the same folder as the app)

	Source - the BACnet Stack code

	AppTest1 - A simple Windows program that reads Devices, Objects, 
		and can read/write Binary properties.

Updates:

November 19, 2013
	Original Issue

December 3, 2013
	Removed the ONE cyclic reference call (used for testing only)
	Removed unused code in AppTest1 (used for testing only)


To Do:
	Bacnet.Ccs: Replace Windows Timer with .NET Timer
	AppTest1: Add Analog Property Read/Write

