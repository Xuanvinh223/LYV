using System;
using System.Xml;
using LYV.Out.UCO;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace LYV.Trigger.Out
{
    public class Out_ID : Ede.Uof.WKF.ExternalUtility.IFormAutoNumber
    {
        public void Finally()
        {
            throw new NotImplementedException();
        }


        public string GetFormNumber(string formId, string userGroupId, string formValueXML)
        {
            OutUCO uco = new OutUCO();
            DateTime date = DateTime.Now; // Sử dụng ngày hiện tại
            string getSerri = uco.GetLNO();

            // Sử dụng Regex để chỉ lấy các ký tự số từ chuỗi getSerri
            string onlyNumbers = Regex.Replace(getSerri, "[^0-9]", "");

            // Kiểm tra nếu chuỗi chỉ chứa số và đủ dài
            if (onlyNumbers.Length < 3)
            {
                throw new InvalidOperationException("Lỗi: Chuỗi chỉ chứa số không đủ 3 ký tự.");
            }

            // Lấy 3 số cuối cùng từ chuỗi đã được làm sạch
            string serri = onlyNumbers.Substring(onlyNumbers.Length - 3);

            // Cố gắng chuyển đổi 3 số cuối thành số nguyên
            if (!int.TryParse(serri, out int lastThreeDigits))
            {
                throw new InvalidOperationException("Lỗi: Không thể chuyển đổi chuỗi thành số nguyên.");
            }

            // Cộng thêm 1 vào số nguyên
            int incrementedValue = lastThreeDigits + 1;

            // Chuyển đổi số nguyên đã cộng thêm 1 trở lại thành chuỗi, đảm bảo độ dài 3 ký tự
            string incrementedStr = incrementedValue.ToString("D3");

            // Tạo chuỗi kết quả theo định dạng mong muốn
            string LNO = "R" + date.Year.ToString() + date.Month.ToString("D2") + date.Day.ToString("D2") + incrementedStr;

            return LNO;
        }

        public void OnError()
        {
            throw new NotImplementedException();
        }

        public void OnExecption(Exception errorException)
        {
            throw new NotImplementedException();
        }
    }
}
