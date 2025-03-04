using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;

namespace NiuMa
{
    public class ResolveState
    {
        private object _lock = new object();
        private bool _resolved = false;

        private string _domain = "";
        private IPHostEntry _resolvedIPs = null;
        private List<string> _ips = null;

        public bool IsResolved()
        {
            lock (_lock)
            {
                return _resolved;
            }
        }

        private void SetResolved(bool s)
        {
            lock (_lock)
            {
                _resolved = s;
            }
        }

        private static void GetHostEntryCallback(IAsyncResult ar)
        {
            try
            {
                ResolveState rs = ar.AsyncState as ResolveState;
                if (rs == null)
                    return;
                rs.SetResolved(true);
                rs._resolvedIPs = Dns.EndGetHostEntry(ar);
            }
            catch (Exception)
            { }
        }

        public void GetHostEntryAsync(string domain)
        {
            try
            {
                _domain = domain;
                Dns.BeginGetHostEntry(domain, new AsyncCallback(GetHostEntryCallback), this);
            }
            catch (Exception)
            { }
        }

        private void WriteIPList()
        {
            if (!IsResolved())
                return ;
            if (_ips == null)
            {
                if (_resolvedIPs == null)
                    return;
                _ips = new List<string>();
            }
            else
                _ips.Clear();
            foreach (IPAddress ip in _resolvedIPs.AddressList)
                _ips.Add(ip.ToString());
        }

        public string GetIPAddress(int idx)
        {
            WriteIPList();
            if (_ips == null)
                return "";
            if (idx >= _ips.Count)
                return "";
            return _ips[idx];
        }

        public int GetIPCount()
        {
            WriteIPList();
            if (_ips == null)
                return 0;

            return _ips.Count;
        }
    }
}