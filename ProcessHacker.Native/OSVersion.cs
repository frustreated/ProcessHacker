﻿/*
 * Process Hacker - 
 *   operating system version information
 *
 * Copyright (C) 2009 wj32
 * 
 * This file is part of Process Hacker.
 * 
 * Process Hacker is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Process Hacker is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Process Hacker.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Text;
using ProcessHacker.Native.Security;

namespace ProcessHacker.Native
{
    public enum WindowsVersion
    {
        /// <summary>
        /// Windows XP SP2 and SP3.
        /// </summary>
        XP = 51,

        /// <summary>
        /// Windows Server 2003.
        /// </summary>
        Server2003 = 52,

        /// <summary>
        /// Windows Vista SP0 and SP1, Windows Server 2008.
        /// </summary>
        Vista = 60,

        /// <summary>
        /// Windows 7 SP0.
        /// </summary>
        Seven = 61,

        /// <summary>
        /// An unreleased version of Windows.
        /// </summary>
        Unreleased = int.MaxValue
    }

    public static class OSVersion
    {
        private static WindowsVersion _windowsVersion;

        private static ProcessAccess _minProcessQueryInfoAccess = ProcessAccess.QueryInformation;
        private static ThreadAccess _minThreadQueryInfoAccess = ThreadAccess.QueryInformation;
        private static ThreadAccess _minThreadSetInfoAccess = ThreadAccess.SetInformation;

        private static bool _hasCycleTime = false;
        private static bool _hasProtectedProcesses = false;
        private static bool _hasPsSuspendResumeProcess = false;
        private static bool _hasQueryLimitedInformation = false;
        private static bool _hasSetAccessToken = false;
        private static bool _hasTaskDialogs = false;
        private static bool _hasUac = false;
        private static bool _hasWin32ImageFileName = false;

        static OSVersion()
        {
            System.Version version = Environment.OSVersion.Version;

            if (version.Major == 5 && version.Minor == 1)
                _windowsVersion = WindowsVersion.XP;
            else if (version.Major == 5 && version.Minor == 2)
                _windowsVersion = WindowsVersion.Server2003;
            else if (version.Major == 6 && version.Minor == 0)
                _windowsVersion = WindowsVersion.Vista;
            else if (version.Major == 6 && version.Minor == 1)
                _windowsVersion = WindowsVersion.Seven;
            else if ((version.Major == 6 && version.Minor > 1) || version.Major > 6)
                _windowsVersion = WindowsVersion.Unreleased;

            if (IsBelow(WindowsVersion.Vista))
            {
                _hasSetAccessToken = true;
            }

            if (IsAboveOrEqual(WindowsVersion.Vista))
            {
                _minProcessQueryInfoAccess = ProcessAccess.QueryLimitedInformation;
                _minThreadQueryInfoAccess = ThreadAccess.QueryLimitedInformation;
                _minThreadSetInfoAccess = ThreadAccess.SetLimitedInformation;

                _hasCycleTime = true;
                _hasProtectedProcesses = true;
                _hasPsSuspendResumeProcess = true;
                _hasQueryLimitedInformation = true;
                _hasTaskDialogs = true;
                _hasUac = true;
                _hasWin32ImageFileName = true;
            }
        }

        public static WindowsVersion WindowsVersion
        {
            get { return _windowsVersion; }
        }

        public static ProcessAccess MinProcessQueryInfoAccess
        {
            get { return _minProcessQueryInfoAccess; }
        }

        public static ThreadAccess MinThreadQueryInfoAccess
        {
            get { return _minThreadQueryInfoAccess; }
        }

        public static ThreadAccess MinThreadSetInfoAccess
        {
            get { return _minThreadSetInfoAccess; }
        }

        public static bool HasCycleTime
        {
            get { return _hasCycleTime; }
        }

        public static bool HasProtectedProcesses
        {
            get { return _hasProtectedProcesses; }
        }

        public static bool HasPsSuspendResumeProcess
        {
            get { return _hasPsSuspendResumeProcess; }
        }

        public static bool HasQueryLimitedInformation
        {
            get { return _hasQueryLimitedInformation; }
        }

        public static bool HasSetAccessToken
        {
            get { return _hasSetAccessToken; }
        }

        public static bool HasTaskDialogs
        {
            get { return _hasTaskDialogs; }
        }

        public static bool HasUac
        {
            get { return _hasUac; }
        }

        public static bool HasWin32ImageFileName
        {
            get { return _hasWin32ImageFileName; }
        }

        public static bool IsAbove(WindowsVersion version)
        {
            return (int)_windowsVersion > (int)version;
        }

        public static bool IsAboveOrEqual(WindowsVersion version)
        {
            return (int)_windowsVersion >= (int)version;
        }

        public static bool IsBelowOrEqual(WindowsVersion version)
        {
            return (int)_windowsVersion <= (int)version;
        }

        public static bool IsBelow(WindowsVersion version)
        {
            return (int)_windowsVersion < (int)version;
        }
    }
}
