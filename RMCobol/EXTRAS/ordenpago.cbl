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
            SELECT ORDENPAGO
              ASSIGN TO RANDOM "F-ORD-COMP"
              ORGANIZATION IS INDEXED
              ACCESS MODE IS DYNAMIC
              RECORD KEY IS ORDENPAGO-KEY
              ALTERNATE RECORD KEY IS
                  ORDENPAGO-PROV-KEY = ORDENPAGO-PROVE, ORDENPAGO-ID
              ALTERNATE RECORD KEY IS
                  ORDENPAGO-FEC-KEY = ORDENPAGO-FECHA, ORDENPAGO-ID
              ALTERNATE RECORD KEY IS
                  ORDENPAGO-PEND-KEY = ORDENPAGO-PEND, ORDENPAGO-ID
              FILE STATUS IS FS-COMUN.

           SELECT SECUENCIAL
                   ASSIGN TO RANDOM "ORDENPAGO.sec"
                   ORGANIZATION IS LINE SEQUENTIAL
                   ACCESS MODE IS SEQUENTIAL
                   FILE STATUS IS ST.
       DATA DIVISION.
       FILE SECTION.
        FD  ORDENPAGO
            LABEL RECORD IS STANDARD.
        01  REG-ORDENPAGO.
            02 ORDENPAGO-KEY.
               03 ORDENPAGO-ID           PIC 9(06).
            02 ORDENPAGO-PROVE           PIC 9(04).
            02 ORDENPAGO-FECHA           PIC 9(08).
            02 ORDENPAGO-PEND            PIC 9(01).
            02 ORDENPAGO-PAGO            PIC 9(02)       COMP-6.
            02 ORDENPAGO-DTO-1           PIC 9(02)V9(02) COMP-6.
            02 ORDENPAGO-DTO-2           PIC 9(02)V9(02) COMP-6.
            02 ORDENPAGO-DTO-3           PIC 9(02)V9(02) COMP-6.
            02 ORDENPAGO-ANTIC           PIC X(01).
            02 ORDENPAGO-DEPO            PIC 9(04)       COMP-6.
            02 ORDENPAGO-OBS-1           PIC X(60).
            02 ORDENPAGO-OBS-2           PIC X(60).
            02 ORDENPAGO-OBS-3           PIC X(60).
            02 ORDENPAGO-TOTAL           PIC 9(10)V9(02) COMP-6.
            02 ORDENPAGO-IVA             PIC 9(10)V9(02) COMP-6.
            02 ORDENPAGO-OTROS           PIC 9(10)V9(02) COMP-6.
            02 ORDENPAGO-SI-IMPRE        PIC X(01).
            02 ORDENPAGO-EMPRESA         PIC 9(02)       COMP-6.
            02 ORDENPAGO-MONEDA          PIC 9(02)       COMP-6.
            02 ORDENPAGO-COTIZ           PIC 9(04)V9(04) COMP-6.
            02 ORDENPAGO-DOLAR           PIC 9(02)V9(04) COMP-6.
            02 ORDENPAGO-AUTORIZA        PIC X(01).
            02 ORDENPAGO-TABLA.
               03 ORDENPAGO-IT OCCURS 5.
                  04 ORDENPAGO-FEC-ENT   PIC 9(06)       COMP-6.
            02 ORDENPAGO-PCIA-IBB        PIC 9(02).
            02 ORDENPAGO-CONFIR          PIC X(01).
            02 ORDENPAGO-TIPO            PIC 9(01).
            02 ORDENPAGO-PORC            PIC 9(03)V9(02).
            02 ORDENPAGO-RESTO           PIC X(02).
            FD  secuencial
                RECORD VARYING FROM 0 TO 900 CHARACTERS
                DEPENDING LNG.
       01 REG-SS.
            02 SEC-ID              PIC 9(06).
            02 PIC X(01).
            02 SEC-PROVE           PIC 9(04).
            02 PIC X(01).
            02 SEC-FECHA           PIC 9(08).
            02 PIC X(01).
            02 SEC-PEND            PIC 9(01).
            02 PIC X(01).
            02 SEC-PAGO            PIC 9(02)       .
            02 PIC X(01).
            02 SEC-DTO-1           PIC 9(02)V9(02) .
            02 PIC X(01).
            02 SEC-DTO-2           PIC 9(02)V9(02) .
            02 PIC X(01).
            02 SEC-DTO-3           PIC 9(02)V9(02) .
            02 PIC X(01).
            02 SEC-ANTIC           PIC X(01).
            02 PIC X(01).
            02 SEC-DEPO            PIC 9(04)       .
            02 PIC X(01).
            02 SEC-OBS-1           PIC X(60).
            02 PIC X(01).
            02 SEC-OBS-2           PIC X(60).
            02 PIC X(01).
            02 SEC-OBS-3           PIC X(60).
            02 PIC X(01).
            02 SEC-TOTAL           PIC 9(10)V9(02) .
            02 PIC X(01).
            02 SEC-IVA             PIC 9(10)V9(02) .
            02 PIC X(01).
            02 SEC-OTROS           PIC 9(10)V9(02) .
            02 PIC X(01).
            02 SEC-SI-IMPRE        PIC X(01).
            02 PIC X(01).
            02 SEC-EMPRESA         PIC 9(02)       .
            02 PIC X(01).
            02 SEC-MONEDA          PIC 9(02)       .
            02 PIC X(01).
            02 SEC-COTIZ           PIC 9(04)V9(04) .
            02 PIC X(01).
            02 SEC-DOLAR           PIC 9(02)V9(04) .
            02 PIC X(01).
            02 SEC-AUTORIZA        PIC X(01).
            02 PIC X(01).
            02 SEC-FEC-ENT-1       PIC 9(06)       .
            02 PIC X(01).
            02 SEC-FEC-ENT-2       PIC 9(06)       .
            02 PIC X(01).
            02 SEC-FEC-ENT-3       PIC 9(06)       .
            02 PIC X(01).
            02 SEC-FEC-ENT-4       PIC 9(06)       .
            02 PIC X(01).
            02 SEC-FEC-ENT-5       PIC 9(06)       .
            02 PIC X(01).
            02 SEC-PCIA-IBB        PIC 9(02).
            02 PIC X(01).
            02 SEC-CONFIR          PIC X(01).
            02 PIC X(01).
            02 SEC-TIPO            PIC 9(01).
            02 PIC X(01).
            02 SEC-PORC            PIC 9(03)V9(02).
            02 PIC X(01).
            02 SEC-RESTO           PIC X(02).
            02 PIC X(01).
       WORKING-STORAGE SECTION.
       01  LNG                    PIC 999 VALUE 317.
       01  CANTIDAD               PIC 99999999 VALUE ZEROS.
       01  ERRORES                PIC 99999999 VALUE ZEROS.
       01  ed-cantidad            pic ZZZ.ZZZ.ZZZ.
       01  EMPRESA                PIC 99.
       01  ST                     PIC X(02).
       01  FS-COMUN               PIC X(02).
       77  EOF-ORDENPAGO          PIC X(01).
       77  EXISTE-ORDENPAGO       PIC X(01).
       77  TOTAL                  PIC 9(08) VALUE 0.
       PROCEDURE DIVISION.
       INICIO.
           DISPLAY "(S)ecuencializar - (I)ndexar ? : " LINE 0 POSITION 0
           ACCEPT EOF-ORDENPAGO PROMPT ECHO        LINE 0 POSITION 0
           IF EOF-ORDENPAGO = "S" OR = "s"
              OPEN  INPUT ORDENPAGO
                    OUTPUT secuencial
                    PERFORM SECUENCIALIZAR
           ELSE IF EOF-ORDENPAGO = "I" OR "i"
              OPEN  OUTPUT ORDENPAGO
                    INPUT  secuencial
                    PERFORM INDEXAR.
           CLOSE ORDENPAGO secuencial.
           MOVE CANTIDAD TO ED-CANTIDAD
           DISPLAY "Registros Procesados: ", ED-CANTIDAD
           MOVE ERRORES  TO ED-CANTIDAD
           DISPLAY "Registros Erroneos  : ", ED-CANTIDAD
           ACCEPT ST.
           GOBACK.
        SECUENCIALIZAR.
           PERFORM START-ORDENPAGO
           IF EOF-ORDENPAGO = "N"
              PERFORM LEER-ORDENPAGO-NEXT
              PERFORM UNTIL EOF-ORDENPAGO = "S"
                 PERFORM MOVER-CAMPOS-AL-SEC
                 WRITE REG-SS
                 add 1 to cantidad
                 PERFORM LEER-ORDENPAGO-NEXT
              END-PERFORM.
       MOVER-CAMPOS-AL-SEC.
           MOVE SPACES TO REG-SS

            MOVE ORDENPAGO-ID
            TO SEC-ID           .
            MOVE ORDENPAGO-PROVE
            TO SEC-PROVE           .
            MOVE ORDENPAGO-FECHA
            TO SEC-FECHA           .
            MOVE ORDENPAGO-PEND
            TO SEC-PEND            .
            MOVE ORDENPAGO-PAGO
            TO SEC-PAGO            .
            MOVE ORDENPAGO-DTO-1
            TO SEC-DTO-1           .
            MOVE ORDENPAGO-DTO-2
            TO SEC-DTO-2           .
            MOVE ORDENPAGO-DTO-3
            TO SEC-DTO-3           .
            MOVE ORDENPAGO-ANTIC
            TO SEC-ANTIC           .
            MOVE ORDENPAGO-DEPO
            TO SEC-DEPO            .
            MOVE ORDENPAGO-OBS-1
            TO SEC-OBS-1           .
            MOVE ORDENPAGO-OBS-2
            TO SEC-OBS-2           .
            MOVE ORDENPAGO-OBS-3
            TO SEC-OBS-3           .
            MOVE ORDENPAGO-TOTAL
            TO SEC-TOTAL           .
            MOVE ORDENPAGO-IVA
            TO SEC-IVA             .
            MOVE ORDENPAGO-OTROS
            TO SEC-OTROS           .
            MOVE ORDENPAGO-SI-IMPRE
            TO SEC-SI-IMPRE        .
            MOVE ORDENPAGO-EMPRESA
            TO SEC-EMPRESA         .
            MOVE ORDENPAGO-MONEDA
            TO SEC-MONEDA          .
            MOVE ORDENPAGO-COTIZ
            TO SEC-COTIZ           .
            MOVE ORDENPAGO-DOLAR
            TO SEC-DOLAR           .
            MOVE ORDENPAGO-AUTORIZA
            TO SEC-AUTORIZA        .
            MOVE ORDENPAGO-FEC-ENT(1)   TO SEC-FEC-ENT-1.
            MOVE ORDENPAGO-FEC-ENT(2)   TO SEC-FEC-ENT-2.
            MOVE ORDENPAGO-FEC-ENT(3)   TO SEC-FEC-ENT-3.
            MOVE ORDENPAGO-FEC-ENT(4)   TO SEC-FEC-ENT-4.
            MOVE ORDENPAGO-FEC-ENT(5)   TO SEC-FEC-ENT-5.
            MOVE ORDENPAGO-PCIA-IBB
            TO SEC-PCIA-IBB        .
            MOVE ORDENPAGO-CONFIR
            TO SEC-CONFIR          .
            MOVE ORDENPAGO-TIPO
            TO SEC-TIPO            .
            MOVE ORDENPAGO-PORC
            TO SEC-PORC            .
            MOVE ORDENPAGO-RESTO
            TO SEC-RESTO           .

       INDEXAR.
              MOVE "N" TO EOF-ORDENPAGO
              PERFORM LEER-secuencial
              PERFORM UNTIL EOF-ORDENPAGO = "S"
                 PERFORM MOVER-CAMPOS-AL-INX
                 WRITE REG-ORDENPAGO INVALID add 1 to ERRORES
                 END-WRITE
                 add 1 to cantidad
                 PERFORM LEER-secuencial
              END-PERFORM.
       MOVER-CAMPOS-AL-INX.
           MOVE SPACES TO REG-ORDENPAGO
            MOVE SEC-ID
            TO ORDENPAGO-ID           .
            MOVE SEC-PROVE
            TO ORDENPAGO-PROVE           .
            MOVE SEC-FECHA
            TO ORDENPAGO-FECHA           .
            MOVE SEC-PEND
            TO ORDENPAGO-PEND            .
            MOVE SEC-PAGO
            TO ORDENPAGO-PAGO            .
            MOVE SEC-DTO-1
            TO ORDENPAGO-DTO-1           .
            MOVE SEC-DTO-2
            TO ORDENPAGO-DTO-2           .
            MOVE SEC-DTO-3
            TO ORDENPAGO-DTO-3           .
            MOVE SEC-ANTIC
            TO ORDENPAGO-ANTIC           .
            MOVE SEC-DEPO
            TO ORDENPAGO-DEPO            .
            MOVE SEC-OBS-1
            TO ORDENPAGO-OBS-1           .
            MOVE SEC-OBS-2
            TO ORDENPAGO-OBS-2           .
            MOVE SEC-OBS-3
            TO ORDENPAGO-OBS-3           .
            MOVE SEC-TOTAL
            TO ORDENPAGO-TOTAL           .
            MOVE SEC-IVA
            TO ORDENPAGO-IVA             .
            MOVE SEC-OTROS
            TO ORDENPAGO-OTROS           .
            MOVE SEC-SI-IMPRE
            TO ORDENPAGO-SI-IMPRE        .
            MOVE SEC-EMPRESA
            TO ORDENPAGO-EMPRESA         .
            MOVE SEC-MONEDA
            TO ORDENPAGO-MONEDA          .
            MOVE SEC-COTIZ
            TO ORDENPAGO-COTIZ           .
            MOVE SEC-DOLAR
            TO ORDENPAGO-DOLAR           .
            MOVE SEC-AUTORIZA
            TO ORDENPAGO-AUTORIZA        .
            MOVE SEC-FEC-ENT-1     TO ORDENPAGO-FEC-ENT(1).
            MOVE SEC-FEC-ENT-2     TO ORDENPAGO-FEC-ENT(2).
            MOVE SEC-FEC-ENT-3     TO ORDENPAGO-FEC-ENT(3).
            MOVE SEC-FEC-ENT-4     TO ORDENPAGO-FEC-ENT(4).
            MOVE SEC-FEC-ENT-5     TO ORDENPAGO-FEC-ENT(5).
            MOVE SEC-PCIA-IBB
            TO ORDENPAGO-PCIA-IBB        .
            MOVE SEC-CONFIR
            TO ORDENPAGO-CONFIR          .
            MOVE SEC-TIPO
            TO ORDENPAGO-TIPO            .
            MOVE SEC-PORC
            TO ORDENPAGO-PORC            .
            MOVE SEC-RESTO
            TO ORDENPAGO-RESTO           .
       START-ORDENPAGO.
           MOVE LOW-VALUE TO REG-ORDENPAGO
           MOVE "N" TO EOF-ORDENPAGO.
           START ORDENPAGO
                       KEY NOT < ORDENPAGO-KEY
                                      INVALID KEY
                                MOVE "S" TO EOF-ORDENPAGO.
       LEER-ORDENPAGO-NEXT.
           MOVE "N" TO EOF-ORDENPAGO.
           READ ORDENPAGO NEXT
                           AT END
                                 MOVE "S" TO EOF-ORDENPAGO.
       LEER-secuencial.
           READ secuencial AT END
                MOVE "S" TO EOF-ORDENPAGO.
