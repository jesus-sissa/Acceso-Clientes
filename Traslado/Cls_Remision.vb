Public Class Cls_Remision
    Inherits BasePage

    Function Consultar_Mat() As DataTable
        Return cn.fn_ConsultaR1(CInt(Num_Remision))
    End Function
    Function Consultar_Tras() As DataTable
        Return cn.fn_ConsultaR2(CDbl(Num_Remision))
    End Function
End Class
