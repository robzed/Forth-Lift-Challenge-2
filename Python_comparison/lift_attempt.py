import os
import time

class Lift:
    def __init__(self, floors):
        self.floors = floors
        self.lift_position = 1
        self.going_up = True
        self.requests_up = set()
        self.requests_down = set()
        self.destinations = set()

    def request(self, floor, direction):
        if direction == "UP":
            self.requests_up.add(floor)
        elif direction == "DOWN":
            self.requests_down.add(floor)

    def select_floor(self, floor):
        self.destinations.add(floor)

    def move(self):
        if self.going_up:
            if self.lift_position in self.destinations:
                self.destinations.remove(self.lift_position)
                print(f"Stopping at floor {self.lift_position}")
            if self.lift_position in self.requests_up:
                self.requests_up.remove(self.lift_position)
                print(f"Picking up passengers going up from floor {self.lift_position}")

            if self.lift_position < self.floors:
                self.lift_position += 1
            else:
                self.going_up = False
        else:
            if self.lift_position in self.destinations:
                self.destinations.remove(self.lift_position)
                print(f"Stopping at floor {self.lift_position}")
            if self.lift_position in self.requests_down:
                self.requests_down.remove(self.lift_position)
                print(f"Picking up passengers going down from floor {self.lift_position}")

            if self.lift_position > 1:
                self.lift_position -= 1
            else:
                self.going_up = True

    def display(self):
        os.system('cls' if os.name == 'nt' else 'clear')
        print("LIFT SIMULATOR")
        print("#  UP DN STOP   LIFT")
        print("===========================")
        for i in range(self.floors, 0, -1):
            up = "#" if i in self.requests_up else " "
            down = "#" if i in self.requests_down else " "
            stop = "#" if i in self.destinations else " "
            lift = "  | [UP] |" if self.lift_position == i and self.going_up else "  |      |"
            lift = "  | [DN] |" if self.lift_position == i and not self.going_up else lift
            print(f"{i:2} {up}  {down} {stop}  {lift}")
            print("--------------|      |--")

def simulate_lift(lift):
    while True:
        lift.display()
        print("\nEnter 'floor_number UP/DOWN' for floor requests, 'floor_number GO' to select a floor inside the lift, or 'q' to quit.")
        command = input().strip().upper()
        if command == 'Q':
            break
        elif len(command.split()) == 2:
            floor, action = command.split()
            floor = int(floor)
            if action in ["UP", "DOWN"]:
                lift.request(floor, action)
            elif action == "GO":
                lift.select_floor(floor)
        lift.move()

if __name__ == "__main__":
    lift = Lift(6)
    simulate_lift(lift)
