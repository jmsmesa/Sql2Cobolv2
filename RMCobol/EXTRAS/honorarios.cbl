       IDENTIFICATION DIVISION.
       PROGRAM-ID.             Edit.
       ENVIRONMENT DIVISION.
       CONFIGURATION SECTION.
       SOURCE-COMPUTER. MULTIPLATAFORMA.
       OBJECT-COMPUTER. MULTIPLATAFORMA.
       SPECIAL-NAMES.
                     DECIMAL-POINT IS COMMA.
       INPUT-OUTPUT SECTION.
       FILE-CONTROL.
            SELECT HONORARIOS
              ASSIGN TO RANDOM "F-HONORA"
              ORGANIZATION IS INDEXED
              ACCESS MODE IS DYNAMIC
              RECORD KEY IS HONORARIOS-KEY
              ALTERNATE RECORD KEY IS HONORARIOS-ALT-LLAVE =
                        HONORARIOS-FECHA, HONORARIOS-ID
              ALTERNATE RECORD KEY IS HONORARIOS-ALT-APM   =
                        HONORARIOS-APM, HONORARIOS-ID
              FILE STATUS IS FS-COMUN.
           SELECT SECUENCIAL
                   ASSIGN TO RANDOM "honorarios.sec"
                   ORGANIZATION IS LINE SEQUENTIAL
                   ACCESS MODE IS SEQUENTIAL
                   FILE STATUS IS ST.
       DATA DIVISION.
       FILE SECTION.
        FD  HONORARIOS
            LABEL RECORD IS STANDARD.
        01  REG-HONORARIOS.
            02 HONORARIOS-KEY.
               03 HONORARIOS-ID           PIC 9(06).
            02 HONORARIOS-APM             PIC 9(02).
            02 HONORARIOS-FECHA           PIC 9(06).
            02 HONORARIOS-AUTOR           PIC 9(01).
            02 HONORARIOS-IMPUE           PIC 9(01).
            02 HONORARIOS-NODONA          PIC 9(01).
            02 HONORARIOS-RESTO           PIC X(37).
            FD  secuencial
                RECORD VARYING FROM 0 TO 900 CHARACTERS
                DEPENDING LNG.
       01 REG-SS.
            02 SEC-ID           PIC 9(06).
            02 PIC X(01).
            02 SEC-APM             PIC 9(02).
            02 PIC X(01).
            02 SEC-FECHA           PIC 9(06).
            02 PIC X(01).
            02 SEC-AUTOR           PIC 9(01).
            02 PIC X(01).
            02 SEC-IMPUE           PIC 9(01).
            02 PIC X(01).
            02 SEC-NODONA          PIC 9(01).
            02 PIC X(01).
            02 SEC-RESTO           PIC X(37).
            02 PIC X(01).
       WORKING-STORAGE SECTION.
       01  LNG                    PIC 999 VALUE 61.
       01  CANTIDAD               PIC 99999999 VALUE ZEROS.
       01  ERRORES                PIC 99999999 VALUE ZEROS.
       01  ed-cantidad            pic ZZZ.ZZZ.ZZZ.
       01  EMPRESA                PIC 99.
       01  ST                     PIC X(02).
       77  EOF-honorarios                 PIC X(01).
       77  EXISTE-honorarios              PIC X(01).
       77  TOTAL                         PIC 9(08) VALUE 0.
       PROCEDURE DIVISION.
       INICIO.
           DISPLAY "(S)ecuencializar - (I)ndexar ? : " LINE 0 POSITION 0
           ACCEPT EOF-honorarios PROMPT ECHO        LINE 0 POSITION 0
           IF EOF-honorarios = "S" OR = "s"
              OPEN  INPUT honorarios
                    OUTPUT secuencial
                    PERFORM SECUENCIALIZAR
           ELSE IF EOF-honorarios = "I" OR "i"
              OPEN  OUTPUT honorarios
                    INPUT  secuencial
                    PERFORM INDEXAR.
           CLOSE honorarios secuencial.
           MOVE CANTIDAD TO ED-CANTIDAD
           DISPLAY "Registros Procesados: ", ED-CANTIDAD
           MOVE ERRORES  TO ED-CANTIDAD
           DISPLAY "Registros Erroneos  : ", ED-CANTIDAD
           ACCEPT ST.
           GOBACK.
        SECUENCIALIZAR.
           PERFORM START-honorarios
           IF EOF-honorarios = "N"
              PERFORM LEER-honorarios-NEXT
              PERFORM UNTIL EOF-honorarios = "S"
                 PERFORM MOVER-CAMPOS-AL-SEC
                 WRITE REG-SS
                 add 1 to cantidad
                 PERFORM LEER-honorarios-NEXT
              END-PERFORM.
       MOVER-CAMPOS-AL-SEC.
           MOVE SPACES TO REG-SS
       
            MOVE honorarios-ID            
            TO SEC-ID           .
            MOVE honorarios-APM              
            TO SEC-APM             .
            MOVE honorarios-FECHA            
            TO SEC-FECHA           .
            MOVE honorarios-AUTOR            
            TO SEC-AUTOR           .
            MOVE honorarios-IMPUE            
            TO SEC-IMPUE           .
            MOVE honorarios-NODONA           
            TO SEC-NODONA          .
            MOVE honorarios-RESTO            
            TO SEC-RESTO           .

       INDEXAR.
              MOVE "N" TO EOF-honorarios
              PERFORM LEER-secuencial
              PERFORM UNTIL EOF-honorarios = "S"
                 PERFORM MOVER-CAMPOS-AL-INX
                 WRITE REG-honorarios INVALID add 1 to ERRORES
                 END-WRITE
                 add 1 to cantidad
                 PERFORM LEER-secuencial
              END-PERFORM.
       MOVER-CAMPOS-AL-INX.
           MOVE SPACES TO REG-honorarios
            MOVE SEC-ID            
            TO honorarios-ID           .
            MOVE SEC-APM              
            TO honorarios-APM             .
            MOVE SEC-FECHA            
            TO honorarios-FECHA           .
            MOVE SEC-AUTOR            
            TO honorarios-AUTOR           .
            MOVE SEC-IMPUE            
            TO honorarios-IMPUE           .
            MOVE SEC-NODONA           
            TO honorarios-NODONA          .
            MOVE SEC-RESTO            
            TO honorarios-RESTO           .
       START-honorarios.
           MOVE LOW-VALUE TO REG-honorarios
           MOVE "N" TO EOF-honorarios.
           START honorarios
                       KEY NOT < honorarios-KEY
                                      INVALID KEY
                                MOVE "S" TO EOF-honorarios.
       LEER-honorarios-NEXT.
           MOVE "N" TO EOF-honorarios.
           READ honorarios NEXT
                           AT END
                                 MOVE "S" TO EOF-honorarios.
       LEER-secuencial.
           READ secuencial AT END
                MOVE "S" TO EOF-honorarios.
