using projAPI.Model;
using projContext;
using projContext.DB.CRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Services
{
    public interface IsrvWallet
    {
        double GetWalletBalance(int CustomerId, ulong Nid, int EmployeeId, enmCustomerType customerType);
        bool UpdateWallet(int CustomerId, ulong Nid, int EmployeeId, enmCustomerType customerType, int UserId, enmTransactionType TransactionType, bool IsAdd, double Amount, ulong PaymentRequestId, string Remarks, string TransationDetails, DateTime TransactionDt);
    }

    public class srvWallet : IsrvWallet
    {
        private readonly CrmContext _crmContxet;
        public srvWallet(CrmContext crmContxet)
        {
            _crmContxet = crmContxet;
        }

        private mdlReturnData GetWalletData(int CustomerId, ulong Nid, int EmployeeId, enmCustomerType customerType)
        {
            mdlReturnData mdl = new mdlReturnData() { MessageType = enmMessageType.None };
            tblCustomerWalletAmount tempdata = null;
            if (customerType == enmCustomerType.MLM)
            {
                tempdata = _crmContxet.tblCustomerWalletAmount.Where(p => p.Nid == Nid).FirstOrDefault();
            }
            else if (customerType == enmCustomerType.InHouse)
            {
                tempdata = _crmContxet.tblCustomerWalletAmount.Where(p => p.EmployeeId == EmployeeId).FirstOrDefault();
            }
            else if (customerType == enmCustomerType.B2C || customerType == enmCustomerType.B2B)
            {
                tempdata = _crmContxet.tblCustomerWalletAmount.Where(p => p.CustomerId == CustomerId).FirstOrDefault();
            }
            if (tempdata == null)
            {
                tempdata = new tblCustomerWalletAmount()
                {
                    CustomerId = CustomerId,
                    EmployeeId = EmployeeId,
                    Nid = Nid,
                    WalletAmount = 0
                };
                _crmContxet.tblCustomerWalletAmount.Add(tempdata);
                _crmContxet.SaveChanges();
            }
            mdl.ReturnId = tempdata;
            mdl.MessageType = enmMessageType.Success;
            return mdl;
        }

        public double GetWalletBalance(int CustomerId, ulong Nid, int EmployeeId, enmCustomerType customerType)
        {
            double WalletBalance = 0;
            var tempData = GetWalletData(CustomerId, Nid, EmployeeId, customerType);
            WalletBalance = tempData.ReturnId.WalletAmount;
            return WalletBalance;
        }

        public bool UpdateWallet(int CustomerId, ulong Nid, int EmployeeId,
            enmCustomerType customerType, int UserId, enmTransactionType TransactionType,
            bool IsAdd, double Amount, ulong PaymentRequestId, string Remarks, string TransationDetails,
            DateTime TransactionDt
            )
        {

            tblCustomerWalletAmount tempdata = GetWalletData(CustomerId, Nid, EmployeeId, customerType).ReturnId;
            if (!IsAdd)
            {
                if (tempdata.WalletAmount < Amount)
                {
                    throw new Exception(enmMessage.InsufficientBalance.GetDescription());
                }
                else
                {
                    tempdata.WalletAmount = tempdata.WalletAmount - Amount;
                }
            }
            else
            {
                tempdata.WalletAmount = tempdata.WalletAmount + Amount;
            }
            _crmContxet.tblCustomerWalletAmount.Update(tempdata);

            tblWalletDetailLedger WalletDetails = new tblWalletDetailLedger()
            {
                TransactionDt = TransactionDt,
                CustomerId = CustomerId,
                Nid = Nid,
                EmployeeId = EmployeeId,
                Credit = IsAdd ? Amount : 0,
                Debit = (!IsAdd) ? Amount : 0,
                TransactionType = TransactionType,
                TransactionDetails = TransationDetails,
                Remarks = Remarks,
                PaymentRequestId = PaymentRequestId,
            };
            _crmContxet.tblWalletDetailLedger.Add(WalletDetails);
            _crmContxet.SaveChanges();
            return true;
        }
    }
}
