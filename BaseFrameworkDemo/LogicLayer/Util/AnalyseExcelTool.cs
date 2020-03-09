using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace LogicLayer.Util
{
    public class AnalyseExcelTool
    {
        public static List<dynamic> GetUserInfoByExcel(string url)
        {
            List<dynamic> list = new List<dynamic>();
            IWorkbook workbook = null;
            try
            {
                workbook = WorkbookFactory.Create(url);
                ISheet sheet = workbook.GetSheetAt(0);
                for (int i = 1; i < sheet.LastRowNum; i++)
                {
                    IRow row = (IRow)sheet.GetRow(i);
                    List<ICell> lICell = row.Cells;
                    if (string.Empty.Equals(GetValue(lICell[1]).ToString()) || GetValue(lICell[1]).ToString() == null) break;//没有输入工号的数据 不进行录入
                    dynamic t = new object();
                    //编号 工号  姓名 性别  手机号 身份证号 民族 邮箱  工种(水电工 / 吊塔)  证书 体检  保险 安全教育考试  公司 部门
                    t.staff_id = GetValue(lICell[1]).ToString();
                    t.username = GetValue(lICell[2]).ToString();
                    t.sex = GetValue(lICell[3]).ToString();
                    t.phone = GetValue(lICell[4]).ToString();
                    t.idcard = GetValue(lICell[5]).ToString();
                    t.nation = GetValue(lICell[6]).ToString();
                    t.email = GetValue(lICell[7]).ToString();
                    t.work_type = GetValue(lICell[8]).ToString();
                    t.certificate = GetValue(lICell[9]).ToString();
                    t.body_examine = GetValue(lICell[10]).ToString();
                    t.insurance = GetValue(lICell[11]).ToString();
                    list.Add(t);
                }
            }
            finally
            {
                workbook?.Close();
            }
            return list;
        }

        private static object GetValue(ICell cell)
        {
            object value = string.Empty;
            if (cell.CellType != CellType.Blank)
            {
                switch (cell.CellType)
                {
                    case CellType.Numeric:
                        if (DateUtil.IsCellDateFormatted(cell))
                        {
                            value = cell.DateCellValue;
                        }
                        else
                        {
                            value = cell.NumericCellValue;
                        }
                        break;
                    case CellType.Boolean:
                        value = cell.BooleanCellValue;
                        break;
                    case CellType.Formula:
                        value = cell.CellFormula;
                        break;
                    default:
                        value = cell.StringCellValue;
                        break;
                }
            }
            return value;
        }

    }
}