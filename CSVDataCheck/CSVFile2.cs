using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVDataCheck
{
    public class CSVFile2
    {
        /// <summary>
        /// [ZH] 設備コード。
        /// </summary>
        public string EquipmentNum { get; set; }

        /// <summary>
        /// [ZH] 工程コード。
        /// </summary>
        public string OperNum { get; set; }

        /// <summary>
        /// [ZH] 構成品番。
        /// </summary>
        public string CmpPrt { get; set; }

        /// <summary>
        /// [ZH] 構成品シリアル番号（単品）。
        /// </summary>
        public string CmpQr { get; set; }

        /// <summary>
        /// [ZH] 構成品シリアル番号（箱）。
        /// </summary>
        public string CmpLot { get; set; }
    }
}
