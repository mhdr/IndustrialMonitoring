using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringAdmin.Lib
{
    public class BACnetItem
    {
        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; }
        }

        public int ItemType
        {
            get { return _itemType; }
            set { _itemType = value; }
        }

        public int SaveInItemsLogTimeInterval
        {
            get { return _saveInItemsLogTimeInterval; }
            set { _saveInItemsLogTimeInterval = value; }
        }

        public int SaveInItemsLogLastesTimeInterval
        {
            get { return _saveInItemsLogLastesTimeInterval; }
            set { _saveInItemsLogLastesTimeInterval = value; }
        }

        public int ShowInUiTimeInterval
        {
            get { return _showInUiTimeInterval; }
            set { _showInUiTimeInterval = value; }
        }

        public int ScanCycle
        {
            get { return _scanCycle; }
            set { _scanCycle = value; }
        }

        public int SaveInItemsLogWhen
        {
            get { return _saveInItemsLogWhen; }
            set { _saveInItemsLogWhen = value; }
        }

        public int SaveInItemsLogLastWhen
        {
            get { return _saveInItemsLogLastWhen; }
            set { _saveInItemsLogLastWhen = value; }
        }

        public int DefinationType
        {
            get { return _definationType; }
            set { _definationType = value; }
        }

        public string Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        public int Order
        {
            get { return _order; }
            set { _order = value; }
        }

        public string BaCnetIp
        {
            get { return _baCnetIp; }
            set { _baCnetIp = value; }
        }

        public int BaCnetPort
        {
            get { return _baCnetPort; }
            set { _baCnetPort = value; }
        }

        public int BaCnetControllerInstance
        {
            get { return _baCnetControllerInstance; }
            set { _baCnetControllerInstance = value; }
        }

        public int BaCnetItemInstance
        {
            get { return _baCnetItemInstance; }
            set { _baCnetItemInstance = value; }
        }

        public int BaCnetItemType
        {
            get { return _baCnetItemType; }
            set { _baCnetItemType = value; }
        }

        public string MinRange
        {
            get { return _minRange; }
            set { _minRange = value; }
        }

        public string MaxRange
        {
            get { return _maxRange; }
            set { _maxRange = value; }
        }

        public bool NormalizeWhenOutOfRange
        {
            get { return _normalizeWhenOutOfRange; }
            set { _normalizeWhenOutOfRange = value; }
        }

        public int ThreadGroupId
        {
            get { return _threadGroupId; }
            set { _threadGroupId = value; }
        }

        public int NumberOfDataForBoxplot
        {
            get { return _numberOfDataForBoxplot; }
            set { _numberOfDataForBoxplot = value; }
        }

        public int InOut
        {
            get { return _inOut; }
            set { _inOut = value; }
        }

        private int _itemType;
        private int _saveInItemsLogTimeInterval;
        private int _saveInItemsLogLastesTimeInterval;
        private int _showInUiTimeInterval;
        private int _scanCycle;
        private int _saveInItemsLogWhen;
        private int _saveInItemsLogLastWhen;
        private int _definationType;
        private string _unit;
        private int _order;
        private string _baCnetIp;
        private int _baCnetPort;
        private int _baCnetControllerInstance;
        private int _baCnetItemInstance;
        private int _baCnetItemType;
        private string _minRange;
        private string _maxRange;
        private bool _normalizeWhenOutOfRange;
        private int _threadGroupId;
        private int _numberOfDataForBoxplot;
        private int _inOut;
        private string _itemName;

        public void LoadAnalogInputDefaultValues()
        {
            this._saveInItemsLogTimeInterval = 30;
            this._saveInItemsLogLastesTimeInterval = 30;
            this._showInUiTimeInterval = 60;
            this._scanCycle = 10000;
            this._saveInItemsLogWhen = 1;
            this._saveInItemsLogLastWhen = 1;
            this._definationType = 3;
            this._baCnetPort = 47808;
        }
    }
}
