using ERP.Contracts.Domain.Core;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ERP.Client.Model
{
    public class DeviceModel : IDevice, INotifyPropertyChanged
    {
        private Guid _deviceId;
        private string _ipAddress;
        private bool _status;
        private string _hostname;
        private string _username;
        private bool _isBloccked;
        private bool _isVerified;
        private EmployeeModel _employee;
        private DivisionModel _division;

        public DeviceModel() { }

        public DeviceModel(DeviceModel device)
        {
            _deviceId = device.DeviceId;
            _ipAddress = device.IpAddress;
            _status = device.Status;
            _hostname = device.Hostname;
            _username = device.Username;
            _isBloccked = device.IsBlocked;
            _isBloccked = device.IsBlocked;
            _isVerified = device.IsVerified;
            _employee = device.Employee;
            _division = device.Division;
        } 

        public Guid DeviceId
        {
            get { return _deviceId; }
            set
            {
                if (_deviceId != value)
                {
                    _deviceId = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string IpAddress
        {
            get { return _ipAddress; }
            set
            {
                if (_ipAddress != value)
                {
                    _ipAddress = value;
                    RaisePropertyChanged();
                }
            }
        }
        public bool Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Hostname
        {
            get { return _hostname; }
            set
            {
                if (_hostname != value)
                {
                    _hostname = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Username
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    RaisePropertyChanged();
                }
            }
        }
        public bool IsBlocked
        {
            get { return _isBloccked; }
            set
            {
                if (_isBloccked != value)
                {
                    _isBloccked = value;
                    RaisePropertyChanged();
                }
            }
        }
        public bool IsVerified
        {
            get { return _isVerified; }
            set
            {
                if (_isVerified != value)
                {
                    _isVerified = value;
                    RaisePropertyChanged();
                }
            }
        }

        public EmployeeModel Employee
        {
            get { return _employee; }
            set
            {
                if (_employee != value)
                {
                    _employee = value;
                    RaisePropertyChanged();
                }
            }
        }

        public DivisionModel Division
        {
            get { return _division; }
            set
            {
                if (_division != value)
                {
                    _division = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int Id { get; set; }

        public int? EmployeeId { get; set; }
        public int? DivisionId { get; set; }

        public static bool operator ==(DeviceModel src, DeviceModel dest)
        {
            if (ReferenceEquals(src, dest))
                return true;

            if ((object)src == null || (object)dest == null)
                return false;

            return  src.DeviceId == dest.DeviceId && 
                    src.Division == dest.Division && 
                    src.DivisionId == dest.DivisionId && 
                    src.Employee == dest.Employee && 
                    src.EmployeeId == dest.EmployeeId && 
                    src.Hostname == dest.Hostname &&
                    src.Id == dest.Id && 
                    src.IpAddress == dest.IpAddress && 
                    src.IsBlocked == dest.IsBlocked && 
                    src.IsVerified == dest.IsVerified && 
                    src.Status == dest.Status && 
                    src.Username == dest.Username;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator !=(DeviceModel src, DeviceModel dest) => !(src == dest);

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
