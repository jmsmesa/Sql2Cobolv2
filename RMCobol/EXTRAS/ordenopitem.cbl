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
            SELECT ORDENOPITEM
              ASSIGN TO RANDOM "F-ITEM-COM"
              ORGANIZATION IS INDEXED
              ACCESS MODE IS DYNAMIC
              RECORD KEY IS ORDENOPITEM-KEY
              FILE STATUS IS FS-COMUN.
           SELECT SECUENCIAL
                   ASSIGN TO RANDOM "ORDENOPITEM.sec"
                   ORGANIZATION IS LINE SEQUENTIAL
                   ACCESS MODE IS SEQUENTIAL
                   FILE STATUS IS ST.
       DATA DIVISION.
       FILE SECTION.
        FD  ORDENOPITEM
            LABEL RECORD IS STANDARD.
        01  REG-ORDENOPITEM.
            02 ORDENOPITEM-KEY.
               03 ORDENOPITEM-ORDEN        PIC 9(06)      COMP-6.
               03 ORDENOPITEM-PRODUCTO     PIC 9(06)      COMP-6.
               03 ORDENOPITEM-TIPO         PIC 9(01).
               03 ORDENOPITEM-SECUENCIA    PIC 9(04)      COMP-6.
            02 ORDENOPITEM-CAMPO           PIC X(170).
            FD  secuencial
                RECORD VARYING FROM 0 TO 900 CHARACTERS
                DEPENDING LNG.
       01 REG-SS.
            02 SEC-ORDEN        PIC 9(06)      .
            02 PIC X(01).
            02 SEC-PRODUCTO     PIC 9(06)      .
            02 PIC X(01).
            02 SEC-TIPO         PIC 9(01).
            02 PIC X(01).
            02 SEC-SECUENCIA    PIC 9(04)      .
            02 PIC X(01).
            02 SEC-CAMPO           PIC X(170).
            02 PIC X(01).
       WORKING-STORAGE SECTION.
       01  LNG                    PIC 999 VALUE 192.
       01  CANTIDAD               PIC 99999999 VALUE ZEROS.
       01  ERRORES                PIC 99999999 VALUE ZEROS.
       01  ed-cantidad            pic ZZZ.ZZZ.ZZZ.
       01  EMPRESA                PIC 99.
       01  ST                     PIC X(02).
       77  EOF-ORDENOPITEM                 PIC X(01).
       77  EXISTE-ORDENOPITEM              PIC X(01).
       77  TOTAL                         PIC 9(08) VALUE 0.
       PROCEDURE DIVISION.
       INICIO.
           DISPLAY "(S)ecuencializar - (I)ndexar ? : " LINE 0 POSITION 0
           ACCEPT EOF-ORDENOPITEM PROMPT ECHO        LINE 0 POSITION 0
           IF EOF-ORDENOPITEM = "S" OR = "s"
              OPEN  INPUT ORDENOPITEM
                    OUTPUT secuencial
                    PERFORM SECUENCIALIZAR
           ELSE IF EOF-ORDENOPITEM = "I" OR "i"
              OPEN  OUTPUT ORDENOPITEM
                    INPUT  secuencial
                    PERFORM INDEXAR.
           CLOSE ORDENOPITEM secuencial.
           MOVE CANTIDAD TO ED-CANTIDAD
           DISPLAY "Registros Procesados: ", ED-CANTIDAD
           MOVE ERRORES  TO ED-CANTIDAD
           DISPLAY "Registros Erroneos  : ", ED-CANTIDAD
           ACCEPT ST.
           GOBACK.
        SECUENCIALIZAR.
           PERFORM START-ORDENOPITEM
           IF EOF-ORDENOPITEM = "N"
              PERFORM LEER-ORDENOPITEM-NEXT
              PERFORM UNTIL EOF-ORDENOPITEM = "S"
                 PERFORM MOVER-CAMPOS-AL-SEC
                 WRITE REG-SS
                 add 1 to cantidad
                 PERFORM LEER-ORDENOPITEM-NEXT
              END-PERFORM.
       MOVER-CAMPOS-AL-SEC.
           MOVE SPACES TO REG-SS

            MOVE ORDENOPITEM-ORDEN
            TO SEC-ORDEN        .
            MOVE ORDENOPITEM-PRODUCTO
            TO SEC-PRODUCTO     .
            MOVE ORDENOPITEM-TIPO
            TO SEC-TIPO         .
            MOVE ORDENOPITEM-SECUENCIA
            TO SEC-SECUENCIA    .
            MOVE ORDENOPITEM-CAMPO
            TO SEC-CAMPO           .

       INDEXAR.
              MOVE "N" TO EOF-ORDENOPITEM
              PERFORM LEER-secuencial
              PERFORM UNTIL EOF-ORDENOPITEM = "S"
                 PERFORM MOVER-CAMPOS-AL-INX
                 WRITE REG-ORDENOPITEM INVALID add 1 to ERRORES
                 END-WRITE
                 add 1 to cantidad
                 PERFORM LEER-secuencial
              END-PERFORM.
       MOVER-CAMPOS-AL-INX.
           MOVE SPACES TO REG-ORDENOPITEM
            MOVE SEC-ORDEN
            TO ORDENOPITEM-ORDEN        .
            MOVE SEC-PRODUCTO
            TO ORDENOPITEM-PRODUCTO     .
            MOVE SEC-TIPO
            TO ORDENOPITEM-TIPO         .
            MOVE SEC-SECUENCIA
            TO ORDENOPITEM-SECUENCIA    .
            MOVE SEC-CAMPO
            TO ORDENOPITEM-CAMPO           .
       START-ORDENOPITEM.
           MOVE LOW-VALUE TO REG-ORDENOPITEM
           MOVE "N" TO EOF-ORDENOPITEM.
           START ORDENOPITEM
                       KEY NOT < ORDENOPITEM-KEY
                                      INVALID KEY
                                MOVE "S" TO EOF-ORDENOPITEM.
       LEER-ORDENOPITEM-NEXT.
           MOVE "N" TO EOF-ORDENOPITEM.
           READ ORDENOPITEM NEXT
                           AT END
                                 MOVE "S" TO EOF-ORDENOPITEM.
       LEER-secuencial.
           READ secuencial AT END
                MOVE "S" TO EOF-ORDENOPITEM.
