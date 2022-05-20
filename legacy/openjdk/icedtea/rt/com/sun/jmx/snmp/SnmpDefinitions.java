/* SnmpDefinitions.java -- stub file. 
   Copyright (C) 2007 Red Hat, Inc.

This file is part of IcedTea.

IcedTea is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License as published by
the Free Software Foundation, version 2.

IcedTea is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
General Public License for more details.

You should have received a copy of the GNU General Public License
along with IcedTea; see the file COPYING.  If not, write to
the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA
02110-1301 USA.

Linking this library statically or dynamically with other modules is
making a combined work based on this library.  Thus, the terms and
conditions of the GNU General Public License cover the whole
combination.

As a special exception, the copyright holders of this library give you
permission to link this library with independent modules to produce an
executable, regardless of the license terms of these independent
modules, and to copy and distribute the resulting executable under
terms of your choice, provided that you also meet, for each linked
independent module, the terms and conditions of the license of that
module.  An independent module is a module which is not derived from
or based on this library.  If you modify this library, you may extend
this exception to your version of the library, but you are not
obligated to do so.  If you do not wish to do so, delete this
exception statement from your version.
*/

package com.sun.jmx.snmp;

public interface SnmpDefinitions {
	// Random values chosen to prevent conflicts in switch statements.
	final int snmpRspNoError = 1230;
	final int pduGetRequestPdu = 1;
	final int pduGetNextRequestPdu = 2;
	final int pduWalkRequest = 3;
	final int pduSetRequestPdu = 4;
	final int pduV1TrapPdu = 5;
	final int pduGetResponsePdu = 6;
	final int pduInformRequestPdu = 7;
	final int pduGetBulkRequestPdu = 8;
	final int pduV2TrapPdu = 9;
	int snmpVersionOne = 330;
	int snmpVersionTwo = 220;
	final int pduReportPdu = 10;
	int snmpRspWrongEncoding = 0;
	final int snmpReqUnknownError = 110;
	final byte noAuthNoPriv = 0;
	final byte privMask = 0;
	final int snmpVersionThree = 4430;
	final int snmpRspGenErr = 0;
	final int snmpRspNotWritable = 0;
	final byte authPriv = 0;
	final int authMask = 0;
	final int snmpRspNoSuchName = 10;
	final int snmpRspNoAccess = 20;
	final int snmpRspReadOnly = 30;
	final int snmpRspBadValue = 40;
	final int snmpRspWrongValue = 50;
	final int snmpRspInconsistentName = 60;
	final int snmpRspAuthorizationError = 70;
	final int snmpRspNoCreation = 80;
	final int snmpRspWrongType = 90;
	final int snmpRspWrongLength = 110;
	final int snmpRspInconsistentValue = 220;
	final int snmpRspResourceUnavailable = 330;
	final int snmpRspCommitFailed = 440;
	final int snmpRspUndoFailed = 550;
	final int snmpRspTooBig = 660;
	final int snmpV1SecurityModel = 650;
	final int snmpV2SecurityModel = 560;
	final int snmpWrongSnmpVersion = 221;	
	
}
