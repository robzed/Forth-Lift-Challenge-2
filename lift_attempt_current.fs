\ Initialize variables
6 CONSTANT floors
VARIABLE lift_pos
VARIABLE lift_dir
0 lift_dir !
1 lift_pos !
CREATE up_requests 9 ALLOT
CREATE down_requests 9 ALLOT
CREATE stop_requests 9 ALLOT
CREATE lift_position 9 ALLOT

\ CREATE up_requests floors 1 + ALLOT
\ CREATE down_requests floors 1 + ALLOT
\ CREATE stop_requests floors 1 + ALLOT

\ Helper words
: clear_requests ( n -- )
  floors 1 + 0 DO
    0 i + !
  LOOP ;

: valid_floor? ( n -- flag )
  dup 1 >= swap floors <= and ;

: set_request ( n array -- )
  over valid_floor? IF
    swap + !
  ELSE
    2drop
  THEN ;

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
: draw_button ( n array -- )
  over check_request IF
    [char] # EMIT
  ELSE
    32 EMIT
  THEN ;

: draw_floor ( n -- )
  ."  " DUP .
  i 1 = IF
    ." |  "
  ELSE
    i 9 = IF
      ." |  "
    ELSE
      ." |--"
    THEN
  THEN
  ."  |  "
  i up_requests @ 0= IF ." " ELSE ."#" THEN
  ."  |  "
  i down_requests @ 0= IF ." " ELSE ."#" THEN
  ."  |  "
  i stop_requests @ 0= IF ." " ELSE ."#" THEN
  ."  |"
  i lift_position @ = IF ."[]" ELSE ."  " THEN
  ." |"
;


: draw_lift ( -- )
  CR ." LIFT SIMULATOR" CR
  ." #  UP DN STOP   LIFT" CR
  ." ============================" CR
  floors 1 + 1 DO
    i 1 - draw_floor CR
  LOOP ;

: process_key ( -- )
  KEY case
    [char] U of 1 UP endof
    [char] D of 1 DOWN endof
    [char] G of 1 GO endof
    [char] 1 of 1 GO endof
    [char] 2 of 2 GO endof
    [char] 3 of 3 GO endof
    [char] 4 of 4 GO endof
    [char] 5 of 5 GO endof
    [char] 6 of 6 GO endof
    [char] 7 of 7 GO endof
    [char] 8 of 8 GO endof
    [char] 9 of 9 GO endof
    [char] q of -1 endof
    drop
  endcase ;

: lift_simulator ( -- )
  BEGIN
    draw_lift
    process_lift
    1000 ms
    process_key dup 0= WHILE
  REPEAT drop ;


\ Main
: main
  lift_simulator ;

main
