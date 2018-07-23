       identification division.
       program-id. s2cHonorarios.
       author.     sm.
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

           copy honora.sl.

       data division.
       file section.
       fd  request
                  block contains 1 records
                  label record is standard.

       01  reg-requestHonorarios.
           02 ReqHonorarios-id              pic 9(06).
           02                               pic x(01).
           02 ReqHonorarios-apm             pic 9(02).
           02                               pic x(01).
           02 ReqHonorarios-fecha           pic 9(06).
           02                               pic x(01).
           02 ReqHonorarios-autor           pic 9(02).
           02                               pic x(01).
           02 ReqHonorarios-impue           pic 9(01).
           02                               pic x(01).
           02 ReqHonorarios-nodona          pic 9(01).

       fd  response
                  block contains 1 records
                  label record is standard.

       01  reg-responseHonorarios.
           02 ResHonorarios-status          pic x(02).
           02 ResSep0                       pic x(01).
           02 ResHonorarios-proceso         pic x(20).
           02 ResSep1                       pic x(01).
           02 ResHonorarios-archivo         pic x(20).
           02 ResSep2                       pic x(01).
           02 ResHonorarios-operacion       pic x(20).
           02 ResSep3                       pic x(01).
           02 ResHonorarios-request         pic x(30).

           copy honora.fd.

       working-storage section.
       01  fs-comun                         pic x(02).
           88 st-ok                             value "00", "02".
           88 st-validos                        value "10", "22", "23", "00", "94", "99", "24", "34", "90", "30", "96", "46", "02", "21", "35", "37", "98".
           88 eof                               value "10", "23", "94", "46".
           88 clave-invalida                    value "21".
           88 existe                            value "22".
           88 no-existe                         value "23".
           88 arch-lleno                        value "24", "34".
           88 arch-no-abierto                   value "42", "47".
           88 arch-cerrado                      value "41".
           88 arch-no-existe                    value "35".
           88 arch-sin-permisos                 value "37".
           88 error-fisico                      value "30".
           88 error-estructura                  value "98".
           88 sector-ocupado                    value "90", "99".
           88 st-return                         value "ff".

       77  Archivo                          pic x(20).
       77  Operacion                        pic x(20).

       77  NombreRequest                      pic x(80).
       77  NombreResponse                     pic x(80).
       77  retorno                            pic 9(09) binary.
       77  st                                 pic x(02).

       01  Argumentos.
           03 arg-archivo                   pic x(080).
           03 arg-path                      pic x(256).
           03 arg-opcion                    pic x(001).

       linkage section.
       01  args.
           02  argc                         pic s9(5) comp-1.
           02  argv.
               03                           pic x(01) occurs 1 to 4000
                                                  depending on argc.

       procedure division using args.
       declaratives.
       file-honora section.
           use after standard error procedure on honora.
       disp-honora.
           move "honora"      to archivo.
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
              initialize reg-responseHonorarios
              move "|"              to ResSep0 ResSep1 ResSep2 ResSep3
              move st               to ResHonorarios-status
              move "s2c_honorarios" to ResHonorarios-proceso
              move Archivo          to ResHonorarios-archivo
              move Operacion        to ResHonorarios-operacion
              move argv(1:26)       to ResHonorarios-request

              write reg-responseHonorarios
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

      *     display "[" argv(1:26) "]"
      *     display "[" NombreRequest "]"
      *     display "[" NombreResponse "]"
      *      accept st

           perform AbrirRequest
           Perform AbrirHonorarios.

       Procesarrequest.
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
           Perform CerrarHonorarios.

           goback.

       GenerarResponse.
           perform AbrirResponse

           initialize reg-responseHonorarios
           move "|"              to ResSep0 ResSep1 ResSep2 ResSep3
           move "s2c_honorarios" to ResHonorarios-proceso
           move "00"             to ResHonorarios-status
           move spaces           to ResHonorarios-archivo
           move spaces           to ResHonorarios-operacion
           move arg-archivo      to ResHonorarios-request

           perform GrabarResponse.

           perform CerrarResponse.

       ProcesarAlta.
           move Reqhonorarios-id       to honora-id
           move Reqhonorarios-apm      to honora-apm
           move Reqhonorarios-fecha    to honora-fecha
           move Reqhonorarios-autor    to honora-autor
           move Reqhonorarios-impue    to honora-impue
           move Reqhonorarios-nodona   to honora-nodona
           move spaces                 to honora-resto

           perform GrabarHonorarios
           if fs-comun = "22"
              perform ReGrabarHonorarios.
       ProcesarBaja.

       ProcesarModificacion.

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
           write reg-responseHonorarios.

       CerrarResponse.
           move "close     " to operacion
           close response.

      * ---------------------------------------------------
       AbrirHonorarios.
           move "open      " to operacion
           open i-o honora.

       Leerhonorarios.
           move "read      " to operacion
           read honora.

       Regrabarhonorarios.
           move "rewrite   " to operacion
           rewrite honora-reg.

       BorrarHonorarios.
           move "delete    " to operacion
           delete honora.

       GrabarHonorarios.
           move "write     " to operacion
           write honora-reg.

       CerrarHonorarios.
           move "close     " to operacion
           Close honora.

