       identification division.
       program-id. s2cOrden-Cmpr.
       environment division.
       configuration section.
       source-computer. multiplataforma.
       object-computer. multiplataforma
           program collating sequence is upper-lower.

       special-names.
           alphabet upper-lower is
               1 thru 65,
               'A' also 'a', 'B' also 'b', 'C' also 'c', 'D' also 'd',
               'E' also 'e', 'F' also 'f', 'G' also 'g', 'H' also 'h',
               'I' also 'i', 'J' also 'j', 'K' also 'k', 'L' also 'l',
               'M' also 'm', 'N' also 'n', 'O' also 'o', 'P' also 'p',
               'Q' also 'q', 'R' also 'r', 'S' also 's', 'T' also 't',
               'U' also 'u', 'V' also 'v', 'W' also 'w', 'X' also 'x',
               'Y' also 'y', 'Z' also 'z',  92 thru 97, 124 thru 128.

       input-output section.
       file-control.
           select request
                  assign to disc NombreRequest
                  organization is line sequential
                  access is sequential
                  file status is fs-comun.

           select response
                  assign to disc NombreResponse
                  organization is line sequential
                  access is sequential
                  file status is fs-comun.

           copy ord-comp.sl.
           copy item-com.sl.

       data division.
       file section.
       fd  request
                  block contains 1 records
                  label record is standard.

       01  reg-requestOrdenesCmpr.
           03 ReqOrdenesCmpr-TipoRegistro        pic 9(01).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Id         pic 9(06).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Prove      pic 9(04).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Fecha      pic 9(08).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Pend       pic 9(01).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Pago       pic 9(02) .
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Dto-1      pic 9(04),9(02).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Dto-2      pic 9(04),9(02).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Dto-3      pic 9(04),9(02).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Antic      pic x(01).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Depo       pic 9(04) .
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Obs-1      pic x(60).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Obs-2      pic x(60).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Obs-3      pic x(60).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Total      pic 9(12),9(02).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Iva        pic 9(12),9(02).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Otros      pic 9(12),9(02).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Si-impre   pic x(01).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Empresa    pic 9(02).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Moneda     pic 9(02).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Cotiz      pic 9(03),9(04).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Dolar      pic 9(02),9(02).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Autoriza   pic x(01).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Fec-ent-1  pic 9(06).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Fec-ent-2  pic 9(06).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Fec-ent-3  pic 9(06).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Fec-ent-4  pic 9(06).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Fec-ent-5  pic 9(06).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Pcia-ibb   pic 9(02).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Confir     pic x(01).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Tipo       pic 9(01).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Porc       pic 9(03),9(02).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Ord-Comp-Resto      pic x(02).

       01  reg-requestOrdenesCmprItem.
           03 ReqOrdenesCmpr-Item-TipoRegistro   pic 9(01).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Orden      pic 9(06).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Producto   pic 9(06).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Tipo       pic 9(01).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Secuencia  pic 9(04).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Nom-Prod   PIC X(50).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Cantidad-1 PIC 9(10),9(04).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Recibida-1 PIC 9(10),9(04).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Cantidad-2 PIC 9(10),9(04).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Recibida-2 PIC 9(10),9(04).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Cantidad-3 PIC 9(10),9(04).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Recibida-3 PIC 9(10),9(04).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Cantidad-4 PIC 9(10),9(04).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Recibida-4 PIC 9(10),9(04).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Cantidad-5 PIC 9(10),9(04).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Recibida-5 PIC 9(10),9(04).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Unidad     PIC X(04).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Precio     PIC 9(12),9(04).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Dto        PIC 9(10),9(04).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Envase     PIC 9(03).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Cantpre    PIC 9(10),9(04).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Uni-Pre    PIC X(04).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Iva        PIC 9(04),9(02).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Sector     PIC 9(04).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Ord-Tra    PIC 9(06).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Cuenta     PIC 9(12).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Caract     PIC X(01).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Proy       PIC 9(04).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Obs        PIC X(30).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Renglon-1  PIC X(50).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Renglon-2  PIC X(50).
           03                                    pic x(01).
           03 ReqOrdenesCmpr-Item-Com-Res-1      PIC X(70).



       fd  response
                  block contains 1 records
                  label record is standard.

       01  reg-responseOrdenesCmpr.
           02 ResOrdenesCmpr-status             pic x(02).
           02 ResSep0                           pic x(01).
           02 ResOrdenesCmpr-proceso            pic x(20).
           02 ResSep1                           pic x(01).
           02 ResOrdenesCmpr-archivo            pic x(20).
           02 ResSep2                           pic x(01).
           02 ResOrdenesCmpr-operacion          pic x(20).
           02 ResSep3                           pic x(01).
           02 ResOrdenesCmpr-request            pic x(30).

           copy ord-comp.fd.
           copy item-com.fd.

       working-storage section.
       01  fs-comun                          pic x(02).
           88 st-ok                              value "00", "02".
           88 st-validos                         value "10", "22", "23", "00", "94", "99", "24", "34", "90", "30", "96", "46", "02", "21", "35", "37", "98".
           88 eof                                value "10", "23", "94", "46".
           88 clave-invalida                     value "21".
           88 existe                             value "22".
           88 no-existe                          value "23".
           88 arch-lleno                         value "24", "34".
           88 arch-no-abierto                    value "42", "47".
           88 arch-cerrado                       value "41".
           88 arch-no-existe                     value "35".
           88 arch-sin-permisos                  value "37".
           88 error-fisico                       value "30".
           88 error-estructura                   value "98".
           88 sector-ocupado                     value "90", "99".
           88 st-return                          value "ff".

       77  Archivo                            pic x(20).
       77  Operacion                          pic x(20).

       77  NombreRequest                      pic x(80).
       77  NombreResponse                     pic x(80).
       77  retorno                            pic 9(09) binary.
       77  st                                 pic x(02).

       01  Argumentos.
           03 arg-archivo                   pic x(080).
           03 arg-path                      pic x(256).
           03 arg-opcion                    pic x(001).

           copy item-com.wk.

       linkage section.
       01  args.
           02  argc                           pic s9(5) comp-1.
           02  argv.
               03                             pic x(01) occurs 1 to 4000
                                                        depending on argc.

       procedure division using args.
       declaratives.
       file-ord-comp section.
           use after standard error procedure on ord-comp.

       disp-ord-comp.
           move "ord-comp"    to archivo.
           perform mensaje-status.

       file-item-com section.
           use after standard error procedure on item-com.
       disp-item-com.
           move "item-com"    to archivo.
           perform mensaje-status.

       file-request section.
           use after standard error procedure on request.
       disp-request.
           move "request"   to archivo.
           perform mensaje-status.

       file-response section.
           use after standard error procedure on response.
       disp-response.
           move "response"   to archivo.
           perform mensaje-status.

       mensaje-status.
           move fs-comun to st
           if st not = "00" and not = "02" and not = "10" and not = "22" and not = "23"
              open output response
              initialize reg-responseOrdenesCmpr
              move "|"                               to ResSep0 ResSep1 ResSep2 ResSep3
              move st                                to ResOrdenesCmpr-status
              move "s2c_Orden_Cmpr   "               to ResOrdenesCmpr-proceso
              move Archivo                           to ResOrdenesCmpr-archivo
              move Operacion                         to ResOrdenesCmpr-operacion
              move arg-archivo                       to ResOrdenesCmpr-request

              write reg-responseOrdenesCmpr
              close response

              goback.
       f-sentencias.
       end declaratives.

       begin section 1.

       IniciarProceso.
           if argv(1:1) = spaces
              goback
           else
              unstring argv
                delimited by "|"
                   into
                      arg-archivo
                      arg-path
                      arg-opcion
              end-unstring
           end-if

           call "C$SetEnv" USING "RUNPATH", arg-path , retorno.

           string ".\interfases\" arg-archivo ".request" delimited by " " into NombreRequest
           string ".\interfases\" arg-archivo ".response" delimited by " " into NombreResponse

           perform AbrirRequest
           Perform AbrirOrdenesCmpr.
           Perform AbrirOrdenesCmprItem.

       Procesarrequest.
           initialize ord-comp-reg item-com-reg item-com-reg-1 item-com-reg-2

           perform LeerRequest
           perform until eof
              evaluate arg-opcion
                 when "a"
                    perform ProcesarAlta
                 when "b"
                    perform ProcesarBaja
                 when "m"
                    perform ProcesarModificacion
              end-evaluate
              perform LeerRequest
           end-perform.

       TerminarProceso.
           perform GenerarResponse

           perform CerrarRequest
           Perform CerrarOrdenesCmpr.
           Perform CerrarOrdenesCmprItem.

           goback.

       GenerarResponse.
           perform AbrirResponse

           initialize reg-responseOrdenesCmpr
           move "|"                                  to ResSep0 ResSep1 ResSep2 ResSep3
           move "s2c_Orden_Cmpr"                     to ResOrdenesCmpr-proceso
           move "00"                                 to ResOrdenesCmpr-status
           move spaces                               to ResOrdenesCmpr-archivo
           move spaces                               to ResOrdenesCmpr-operacion
           move arg-archivo                          to ResOrdenesCmpr-request

           perform GrabarResponse.

           perform CerrarResponse.

       ProcesarAlta.
           if ReqOrdenesCmpr-TipoRegistro = 1
              move ReqOrdenesCmpr-Ord-Comp-Id        to Ord-Comp-Id
              move ReqOrdenesCmpr-Ord-Comp-Prove     to Ord-Comp-Prove
              move ReqOrdenesCmpr-Ord-Comp-Fecha     to Ord-Comp-Fecha
              move ReqOrdenesCmpr-Ord-Comp-Pend      to Ord-Comp-Pend
              move ReqOrdenesCmpr-Ord-Comp-Pago      to Ord-Comp-Pago
              move ReqOrdenesCmpr-Ord-Comp-Dto-1     to Ord-Comp-Dto-1
              move ReqOrdenesCmpr-Ord-Comp-Dto-2     to Ord-Comp-Dto-2
              move ReqOrdenesCmpr-Ord-Comp-Dto-3     to Ord-Comp-Dto-3
              move ReqOrdenesCmpr-Ord-Comp-Antic     to Ord-Comp-Antic
              move ReqOrdenesCmpr-Ord-Comp-Depo      to Ord-Comp-Depo
              move ReqOrdenesCmpr-Ord-Comp-Obs-1     to Ord-Comp-Obs-1
              move ReqOrdenesCmpr-Ord-Comp-Obs-2     to Ord-Comp-Obs-2
              move ReqOrdenesCmpr-Ord-Comp-Obs-3     to Ord-Comp-Obs-3
              move ReqOrdenesCmpr-Ord-Comp-Total     to Ord-Comp-Total
              move ReqOrdenesCmpr-Ord-Comp-Iva       to Ord-Comp-Iva
              move ReqOrdenesCmpr-Ord-Comp-Otros     to Ord-Comp-Otros
              move ReqOrdenesCmpr-Ord-Comp-Si-impre  to Ord-Comp-Si-impre
              move ReqOrdenesCmpr-Ord-Comp-Empresa   to Ord-Comp-Empresa
              move ReqOrdenesCmpr-Ord-Comp-Moneda    to Ord-Comp-Moneda
              move ReqOrdenesCmpr-Ord-Comp-Cotiz     to Ord-Comp-Cotiz
              move ReqOrdenesCmpr-Ord-Comp-Dolar     to Ord-Comp-Dolar
              move ReqOrdenesCmpr-Ord-Comp-Autoriza  to Ord-Comp-Autoriza
              move ReqOrdenesCmpr-Ord-Comp-Fec-ent-1 to Ord-Comp-Fec-ent(01)
              move ReqOrdenesCmpr-Ord-Comp-Fec-ent-2 to Ord-Comp-Fec-ent(02)
              move ReqOrdenesCmpr-Ord-Comp-Fec-ent-3 to Ord-Comp-Fec-ent(03)
              move ReqOrdenesCmpr-Ord-Comp-Fec-ent-4 to Ord-Comp-Fec-ent(04)
              move ReqOrdenesCmpr-Ord-Comp-Fec-ent-5 to Ord-Comp-Fec-ent(05)
              move ReqOrdenesCmpr-Ord-Comp-Pcia-ibb  to Ord-Comp-Pcia-ibb
              move ReqOrdenesCmpr-Ord-Comp-Confir    to Ord-Comp-Confir
              move ReqOrdenesCmpr-Ord-Comp-Tipo      to Ord-Comp-Tipo
              move ReqOrdenesCmpr-Ord-Comp-Porc      to Ord-Comp-Porc
              move ReqOrdenesCmpr-Ord-Comp-Resto     to Ord-Comp-Resto

              perform GrabarOrdenesCmpr
              if fs-comun = "22"
                 perform ReGrabarOrdenesCmpr
              end-if
           else
              move ReqOrdenesCmpr-Item-Com-Orden     to Item-Com-Orden
              move ReqOrdenesCmpr-Item-Com-Producto  to Item-Com-Producto
              move ReqOrdenesCmpr-Item-Com-Tipo      to Item-Com-Tipo
              move ReqOrdenesCmpr-Item-Com-Secuencia to Item-Com-Secuencia

              move ReqOrdenesCmpr-Item-Com-Nom-Prod   to Item-Com-Nom-Prod
              move ReqOrdenesCmpr-Item-Com-Cantidad-1 to Item-Com-Cantidad(1)
              move ReqOrdenesCmpr-Item-Com-Recibida-1 to Item-Com-Recibida(1)
              move ReqOrdenesCmpr-Item-Com-Cantidad-2 to Item-Com-Cantidad(2)
              move ReqOrdenesCmpr-Item-Com-Recibida-2 to Item-Com-Recibida(2)
              move ReqOrdenesCmpr-Item-Com-Cantidad-3 to Item-Com-Cantidad(3)
              move ReqOrdenesCmpr-Item-Com-Recibida-3 to Item-Com-Recibida(3)
              move ReqOrdenesCmpr-Item-Com-Cantidad-4 to Item-Com-Cantidad(4)
              move ReqOrdenesCmpr-Item-Com-Recibida-4 to Item-Com-Recibida(4)
              move ReqOrdenesCmpr-Item-Com-Cantidad-5 to Item-Com-Cantidad(5)
              move ReqOrdenesCmpr-Item-Com-Recibida-5 to Item-Com-Recibida(5)
              move ReqOrdenesCmpr-Item-Com-Unidad     to Item-Com-Unidad
              move ReqOrdenesCmpr-Item-Com-Precio     to Item-Com-Precio
              move ReqOrdenesCmpr-Item-Com-Dto        to Item-Com-Dto
              move ReqOrdenesCmpr-Item-Com-Envase     to Item-Com-Envase
              move ReqOrdenesCmpr-Item-Com-Cantpre    to Item-Com-Cantpre
              move ReqOrdenesCmpr-Item-Com-Uni-Pre    to Item-Com-Uni-Pre
              move ReqOrdenesCmpr-Item-Com-Iva        to Item-Com-Iva
              move ReqOrdenesCmpr-Item-Com-Sector     to Item-Com-Sector
              move ReqOrdenesCmpr-Item-Com-Ord-Tra    to Item-Com-Ord-Tra
              move ReqOrdenesCmpr-Item-Com-Cuenta     to Item-Com-Cuenta
              move ReqOrdenesCmpr-Item-Com-Caract     to Item-Com-Caract
              move ReqOrdenesCmpr-Item-Com-Proy       to Item-Com-Proy
              move ReqOrdenesCmpr-Item-Com-Obs        to Item-Com-Obs
              move ReqOrdenesCmpr-Item-Com-Renglon-1  to Item-Com-Renglon-1
              move ReqOrdenesCmpr-Item-Com-Renglon-2  to Item-Com-Renglon-2
              move ReqOrdenesCmpr-Item-Com-Res-1      to Item-Com-Res-1

              if Item-Com-Producto > 0
                 move item-com-reg-1                  to item-com-campo
              else
                 move item-com-reg-2                  to item-com-campo
              end-if

              perform GrabarOrdenesCmprItem
              if fs-comun = "22"
                 perform ReGrabarOrdenesCmprItem
              end-if.

       ProcesarBaja.
           if ReqOrdenesCmpr-TipoRegistro = 1
              move ReqOrdenesCmpr-Ord-Comp-Id        to Ord-Comp-Id

              perform BorrarOrdenesCmpr
           else
              move ReqOrdenesCmpr-Item-Com-Orden     to Item-Com-Orden
              move ReqOrdenesCmpr-Item-Com-Producto  to Item-Com-Producto
              move ReqOrdenesCmpr-Item-Com-Tipo      to Item-Com-Tipo
              move ReqOrdenesCmpr-Item-Com-Secuencia to Item-Com-Secuencia

              perform BorrarOrdenesCmprItem.

       ProcesarModificacion.
           if ReqOrdenesCmpr-TipoRegistro = 1
              move ReqOrdenesCmpr-Ord-Comp-Id        to Ord-Comp-Id
              move ReqOrdenesCmpr-Ord-Comp-Prove     to Ord-Comp-Prove
              move ReqOrdenesCmpr-Ord-Comp-Fecha     to Ord-Comp-Fecha
              move ReqOrdenesCmpr-Ord-Comp-Pend      to Ord-Comp-Pend
              move ReqOrdenesCmpr-Ord-Comp-Pago      to Ord-Comp-Pago
              move ReqOrdenesCmpr-Ord-Comp-Dto-1     to Ord-Comp-Dto-1
              move ReqOrdenesCmpr-Ord-Comp-Dto-2     to Ord-Comp-Dto-2
              move ReqOrdenesCmpr-Ord-Comp-Dto-3     to Ord-Comp-Dto-3
              move ReqOrdenesCmpr-Ord-Comp-Antic     to Ord-Comp-Antic
              move ReqOrdenesCmpr-Ord-Comp-Depo      to Ord-Comp-Depo
              move ReqOrdenesCmpr-Ord-Comp-Obs-1     to Ord-Comp-Obs-1
              move ReqOrdenesCmpr-Ord-Comp-Obs-2     to Ord-Comp-Obs-2
              move ReqOrdenesCmpr-Ord-Comp-Obs-3     to Ord-Comp-Obs-3
              move ReqOrdenesCmpr-Ord-Comp-Total     to Ord-Comp-Total
              move ReqOrdenesCmpr-Ord-Comp-Iva       to Ord-Comp-Iva
              move ReqOrdenesCmpr-Ord-Comp-Otros     to Ord-Comp-Otros
              move ReqOrdenesCmpr-Ord-Comp-Si-impre  to Ord-Comp-Si-impre
              move ReqOrdenesCmpr-Ord-Comp-Empresa   to Ord-Comp-Empresa
              move ReqOrdenesCmpr-Ord-Comp-Moneda    to Ord-Comp-Moneda
              move ReqOrdenesCmpr-Ord-Comp-Cotiz     to Ord-Comp-Cotiz
              move ReqOrdenesCmpr-Ord-Comp-Dolar     to Ord-Comp-Dolar
              move ReqOrdenesCmpr-Ord-Comp-Autoriza  to Ord-Comp-Autoriza
              move ReqOrdenesCmpr-Ord-Comp-Fec-ent-1 to Ord-Comp-Fec-ent(01)
              move ReqOrdenesCmpr-Ord-Comp-Fec-ent-2 to Ord-Comp-Fec-ent(02)
              move ReqOrdenesCmpr-Ord-Comp-Fec-ent-3 to Ord-Comp-Fec-ent(03)
              move ReqOrdenesCmpr-Ord-Comp-Fec-ent-4 to Ord-Comp-Fec-ent(04)
              move ReqOrdenesCmpr-Ord-Comp-Fec-ent-5 to Ord-Comp-Fec-ent(05)
              move ReqOrdenesCmpr-Ord-Comp-Pcia-ibb  to Ord-Comp-Pcia-ibb
              move ReqOrdenesCmpr-Ord-Comp-Confir    to Ord-Comp-Confir
              move ReqOrdenesCmpr-Ord-Comp-Tipo      to Ord-Comp-Tipo
              move ReqOrdenesCmpr-Ord-Comp-Porc      to Ord-Comp-Porc
              move ReqOrdenesCmpr-Ord-Comp-Resto     to Ord-Comp-Resto
              perform ReGrabarOrdenesCmpr
           else
              move ReqOrdenesCmpr-Item-Com-Orden     to Item-Com-Orden
              move ReqOrdenesCmpr-Item-Com-Producto  to Item-Com-Producto
              move ReqOrdenesCmpr-Item-Com-Tipo      to Item-Com-Tipo
              move ReqOrdenesCmpr-Item-Com-Secuencia to Item-Com-Secuencia

              move ReqOrdenesCmpr-Item-Com-Nom-Prod   to Item-Com-Nom-Prod
              move ReqOrdenesCmpr-Item-Com-Cantidad-1 to Item-Com-Cantidad(1)
              move ReqOrdenesCmpr-Item-Com-Recibida-1 to Item-Com-Recibida(1)
              move ReqOrdenesCmpr-Item-Com-Cantidad-2 to Item-Com-Cantidad(2)
              move ReqOrdenesCmpr-Item-Com-Recibida-2 to Item-Com-Recibida(2)
              move ReqOrdenesCmpr-Item-Com-Cantidad-3 to Item-Com-Cantidad(3)
              move ReqOrdenesCmpr-Item-Com-Recibida-3 to Item-Com-Recibida(3)
              move ReqOrdenesCmpr-Item-Com-Cantidad-4 to Item-Com-Cantidad(4)
              move ReqOrdenesCmpr-Item-Com-Recibida-4 to Item-Com-Recibida(4)
              move ReqOrdenesCmpr-Item-Com-Cantidad-5 to Item-Com-Cantidad(5)
              move ReqOrdenesCmpr-Item-Com-Recibida-5 to Item-Com-Recibida(5)
              move ReqOrdenesCmpr-Item-Com-Unidad     to Item-Com-Unidad
              move ReqOrdenesCmpr-Item-Com-Precio     to Item-Com-Precio
              move ReqOrdenesCmpr-Item-Com-Dto        to Item-Com-Dto
              move ReqOrdenesCmpr-Item-Com-Envase     to Item-Com-Envase
              move ReqOrdenesCmpr-Item-Com-Cantpre    to Item-Com-Cantpre
              move ReqOrdenesCmpr-Item-Com-Uni-Pre    to Item-Com-Uni-Pre
              move ReqOrdenesCmpr-Item-Com-Iva        to Item-Com-Iva
              move ReqOrdenesCmpr-Item-Com-Sector     to Item-Com-Sector
              move ReqOrdenesCmpr-Item-Com-Ord-Tra    to Item-Com-Ord-Tra
              move ReqOrdenesCmpr-Item-Com-Cuenta     to Item-Com-Cuenta
              move ReqOrdenesCmpr-Item-Com-Caract     to Item-Com-Caract
              move ReqOrdenesCmpr-Item-Com-Proy       to Item-Com-Proy
              move ReqOrdenesCmpr-Item-Com-Obs        to Item-Com-Obs
              move ReqOrdenesCmpr-Item-Com-Renglon-1  to Item-Com-Renglon-1
              move ReqOrdenesCmpr-Item-Com-Renglon-2  to Item-Com-Renglon-2
              move ReqOrdenesCmpr-Item-Com-Res-1      to Item-Com-Res-1

              if Item-Com-Producto > 0
                 move item-com-reg-1                  to item-com-campo
              else
                 move item-com-reg-2                  to item-com-campo
              end-if

              perform ReGrabarOrdenesCmprItem.


      * ---------------------------------------------------
       AbrirRequest.
           move "open      " to operacion
           open input request.

       LeerRequest.
           move "read      " to operacion
           read request.

       CerrarRequest.
           move "close     " to operacion
           close request.

      * ---------------------------------------------------
       AbrirResponse.
           move "open      " to operacion
           open output response.

       GrabarResponse.
           move "write     " to operacion
           write reg-responseOrdenesCmpr.

       CerrarResponse.
           move "close     " to operacion
           close response.

      * ---------------------------------------------------
       AbrirOrdenesCmpr.
           move "open      " to operacion
           open i-o ord-comp.

       LeerOrdenesCmpr.
           move "read      " to operacion
           read ord-comp.

       RegrabarOrdenesCmpr.
           move "rewrite   " to operacion
           rewrite ord-comp-reg.

       BorrarOrdenesCmpr.
           move "delete    " to operacion
           delete ord-comp.

       GrabarOrdenesCmpr.
           move "write     " to operacion
           write ord-comp-reg.

       CerrarOrdenesCmpr.
           move "close     " to operacion
           Close ord-comp.

      * ---------------------------------------------------
       AbrirOrdenesCmprItem.
           move "open      " to operacion
           open i-o item-com.

       LeerOrdenesCmprItem.
           move "read      " to operacion
           read item-com.

       RegrabarOrdenesCmprItem.
           move "rewrite   " to operacion
           rewrite item-com-reg.

       BorrarOrdenesCmprItem.
           move "delete    " to operacion
           delete item-com.

       GrabarOrdenesCmprItem.
           move "write     " to operacion
           write item-com-reg.

       CerrarOrdenesCmprItem.
           move "close     " to operacion
           Close item-com.
