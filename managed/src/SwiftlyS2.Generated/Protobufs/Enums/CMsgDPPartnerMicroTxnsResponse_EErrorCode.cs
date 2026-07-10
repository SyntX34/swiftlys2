namespace SwiftlyS2.Shared.ProtobufDefinitions;

public enum CMsgDPPartnerMicroTxnsResponse_EErrorCode
{
    k_MsgValid = 0,
    k_MsgInvalidAppID = 1,
    k_MsgInvalidPartnerInfo = 2,
    k_MsgNoTransactions = 3,
    k_MsgSQLFailure = 4,
    k_MsgPartnerInfoDiscrepancy = 5,
    k_MsgTransactionInsertFailed = 7,
    k_MsgAlreadyRunning = 8,
    k_MsgInvalidTransactionData = 9,
}