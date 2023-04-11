\ Initialize variables
6 CONSTANT floors
VARIABLE lift_pos
VARIABLE lift_dir
0 lift_dir !
1 lift_pos !
CREATE up_requests floors 1 + ALLOT
CREATE down_requests floors 1 + ALLOT
CREATE stop_requests floors 1 + ALLOT

\ Helper words
: clear_requests ( n -- )
  floors 1 + 0 DO
    0 i + !
  LOOP ;

: set_request ( n array -- )
  swap + !
;

: check_request ( n array -- flag )
  swap + @ ;

\ Button words
: UP ( n -- )
  dup 1 <> IF
    up_requests set_request
  THEN ;

: DOWN ( n -- )
  dup floors <> IF
    down_requests set_request
  THEN ;

: GO ( n -- )
  stop_requests set_request ;

\ Lift logic
: next_floor ( -- n )
  lift_dir @ 0= IF
    lift_pos @ 1 -
  ELSE
    lift_pos @ lift_dir @ + 1 -
  THEN ;

: move_lift ( -- )
  lift_pos @ lift_dir @ + lift_pos ! ;

: change_dir ( -- )
  lift_dir @ 0= IF
    1
  ELSE
    0
  THEN lift_dir ! ;

: process_lift ( -- )
  BEGIN
    lift_pos @ up_requests check_request IF
      change_dir
      move_lift
      lift_pos @ down_requests clear_requests
      lift_pos @ stop_requests clear_requests
    ELSE
      lift_pos @ down_requests check_request IF
        move_lift
        lift_pos @ up_requests clear_requests
        lift_pos @ stop_requests clear_requests
      ELSE
        next_floor up_requests check_request
        next_floor down_requests check_request OR IF
          move_lift
        ELSE
          change_dir
        THEN
      THEN
    THEN
  AGAIN ;

\ Graphics
: draw_floor ( n -- )
  ."  " dup 1 .R ." | " 4 0 DO
    dup i + @ 1 = IF
      i "." EMIT
    ELSE
      32 EMIT
    THEN
  LOOP drop ;

: draw_lift ( -- )
  CR ." LIFT SIMULATOR" CR
  ." #  UP DN STOP   LIFT" CR
  ." ============================" CR
  floors 1 + 1 DO
    i 1 - draw_floor CR
  LOOP ;

: lift_simulator ( -- )
  BEGIN
    draw_lift
    process_lift
    1000 ms
  AGAIN ;

\ Main
: main
  lift_simulator ;

main
