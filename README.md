[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-24ddc0f5d75046c5622901739e7c5dd533143b0c8e959d652212380cedb1ea36.svg)](https://classroom.github.com/a/dQRmt-dR)
# PS2 - Movie Theater 
### Deadline: March ... at 23:59
## Introduction

In this assignment you will assist Tom, Jerry, and their circle of cartoon companions in locating seating at the city's sole movie theater.

Tom, Jerry, and their buddies are planning to catch a film at the city's only cinema, where the seating is organized in a single row, divided into sections by narrow aisles. Each section of seats is delineated by an aisle and has an identical number of seats.

## String Representation of the Movie Theater 
Cinema seats and corridors will be denoted by different characters: 
- ```'O'``` denotes an empty seat,
- ```'X'``` denotes an occupied seat,
- ```'|'``` denotes a corridor.
- ```'S'``` denotes seats that are allocated for cartoon characters from Tom and Jerry's group.

#### Examples:
- 6 empty seats separated by 2 corridors:
```
 OO|OO|OO
```
- 6 seats, first two and the last two are occupied, separated by a corridor:
```
 XXO|OXX
```
Notice that each seat section has equal number of seats.


## PART 1: Find Seats for the Group (25 pts)

In Part 1, there are no special conditions when allocating seats for the members of the cartoon group. Whenever an empty seat ```'O'``` is encountered, it should be immediately allocated to one of the group members.



### A) Return `True` or `False`

For Part A, given the number of cartoon characters in the group and the current seat configuration of the movie theater, you need to check if there is an available seat configuration for the group.

To achieve this, you should implement a function called ```seating_arrangement_exists(num_of_chars, seat_config)``` that takes two arguments as follows:
- ```num_of_chars``` : number of characters in the group (int)
- ```seat_config``` : current seat configuration of the movie theater (string)

Return `True` if it is possible to find enough seats for the whole group, otherwise `False`.


Some examples:
```
>> seating_arrangement_exists(7, 'OO|OX|OO|XX|XO|OO')
True                                              # The solution is 'SS|SX|SS|XX|XS|SO'
```
```
>> seating_arrangement_exists(8, 'OOOO|OXXO|XOXO|OOXX')
True                                              # The solution is 'SSSS|SXXS|XSXS|OOXX'
```
```
>> seating_arrangement_exists(3, 'O|X|X|O')
False                                               # There are no enough available seats.
```

### B) Return Seat Configuration

In Part B, in addition to what you implemented in Part A, you need to return the new seat configuration after allocating seats for the whole group if possible. If it is not possible to allocate enough seats for the entire group, you should return `None`.

**Important Note:** Starting from the leftmost seat, please allocate the first empty seats and do not skip any empty seats. You will be graded based on these solutions. For example, if you want to allocate seats for 3 characters in the seat configuration ```OO|OX|OX```, you should allocate the first three empty seats from the left. The correct solution would be ```SS|SX|OX```. However, ```SS|OX|SX``` will not be accepted as an answer.

You should implement a function called ```get_seating_arrangement(num_of_chars, seat_config)``` that takes two arguments as follows:
- ```num_of_chars``` : number of characters in the group (int)
- ```seat_config``` : current seat configuration of the movie theater (string)

If it is possible to find enough seats for the entire group, your function should return the solution. To denote that you have allocated a seat for a cartoon character, put 'S' in that index. If there is no solution, just return `None`.

Some examples:
```
>> get_seating_arrangement(7, 'OO|OX|OO|XX|XO|OO')
'SS|SX|SS|XX|XS|SO'
```
```
>> get_seating_arrangement(8, 'OOOO|OXXO|XOXO|OOXX')
'SSSS|SXXS|XSXS|OOXX'
```
```
>> get_seating_arrangement(3, 'O|X|X|O')
None                                # There are no enough available seats.
```

**Hint**: You can use the ```replace()``` function to replace occurrences of a character in a string. After you have determined that there is a solution, you can replace the 'O's with 'S's using this function. For further information about the ```replace()``` function, you can refer to the [Python documentation](https://docs.python.org/3/library/stdtypes.html#str.replace) or other relevant resources such as [GeeksforGeeks](https://www.geeksforgeeks.org/python-string-replace/).

## PART 2: Find Seats for the Subgroups (75 pts)

In Part 2, the cartoon group wants to sit together but understands that finding adjacent seats for the whole group can be challenging. Therefore, they decide to split into **k equal subgroups**. Each subgroup will look for adjacent seats for themselves separately, according to the following conditions:

- **Condition 1**: The members of **each subgroup should sit in the same seat section** *without being separated by a corridor*. It is not necessary for the subgroup members to be seated adjacently within the section. Thus, members of a subgroup can be seated anywhere in the section as long as they are within the same section. Different subgroups can also sit in the **same seat section** if there are enough seats available.

  Some example cases:
  - There are 6 cartoon characters in the group. They are split into 2-2-2 subgroups, and the seat configuration is ```OOOO|XOXO```. The solution is ```SSSS|XSXS```, indicating that while the subgroups are in the same section, they don't necessarily sit next to each other within those sections.
  - There are 6 cartoon characters in the group. They are split into 2-2-2 subgroups, and the seat configuration is ```OOO|XXO|OOX```. The first subgroup can sit in the first seat section (the leftmost one), and the second subgroup can sit in the last seat section (the rightmost one). However, if there are not enough available seats for the last subgroup in a single section, there is no solution under this condition.

- **Condition 2**: The members of **each subgroup should sit next to each other** *without being separated by a corridor or other people*. This condition requires that each subgroup finds a contiguous block of seats for themselves within a section, ensuring that all members are adjacent to each other.

  Some example cases:
  - There are 6 cartoon characters in the group. They are split into 2-2-2 subgroups, and the seat configuration is ```OOOO|XXOO```. The solution is ```SSSS|XXSS```, showing that each subgroup sits next to each other in a contiguous block.
  - There are 6 cartoon characters in the group. They are split into 2-2-2 subgroups, and the seat configuration is ```OOOO|XOXO```. Although two subgroups can sit in the first section, the third subgroup cannot find adjacent seats in any section. Thus, there is no solution under this condition.


### A) Consider Condition 1, Return `True` or `False`.

For Part A, given the number of cartoon characters in the group, subgroups and seats in one section and the current seat configuration of the cinema, you will check if you can allocate seats for every subgroup considering **Condition 1**. 

To achieve this, you should implement a function called ```seating_arrangement_exists_C1(num_of_chars, num_of_subgroups, seat_config)``` that takes three arguments as follows:
- ```num_of_chars``` : number of characters in the group (int)
- ```num_of_subgroups``` : number of groups that they want to split into (int)
- ```seat_config``` : current seat configuration of the cinema (string)

If it is possible to find place for every subgroup, your function should return `True`. Otherwise, return `False`.

Some examples:
```
>> seating_arrangement_exists_C1(6, 3, 'OO|OX|OO|XX|XO|OO')
True                                                    # The solution is 'SS|OX|SS|XX|XO|SS'
```
```
>> seating_arrangement_exists_C1(8, 4, 'OOOO|OXXO|XOXO')
True                                                   # The solution is 'SSSS|SXXS|XSXS'
```
```
>> seating_arrangement_exists_C1(10, 2, 'OOOOOO|OOOXXO|XXOOXO')
False                                                    # Only one subgroup can sit in the first seat section
```

### B) Consider Condition 2, Return `True` or `False`.

For Part B, given the number of cartoon characters in the group, subgroups and seats in one section and the current seat configuration of the cinema, you will check if you can allocate seats for every subgroup considering **Condition 2**. 

To achieve this, you should implement a function called ```seating_arrangement_exists_C2(num_of_chars, num_of_subgroups, seat_config)``` that takes three arguments as follows:
- ```num_of_chars``` : number of characters in the group (int)
- ```num_of_subgroups``` : number of groups that they want to split into (int)
- ```seat_config``` : current seat configuration of the cinema (string)

If it is possible to find place for every subgroup, your function should return `True`. Otherwise, return `False`.

Some examples:
```
>> seating_arrangement_exists_C2(6, 3, 'OO|OX|OO|XX|XO|OO')
True                                                    # The solution is 'SS|OX|SS|XX|XO|SS'.
```
```
>> seating_arrangement_exists_C2(8, 4, 'OXXO|OXXO|OOOO')
False                                                    # Only two subgroups can sit in the 3rd seat section.
```
```
>> seating_arrangement_exists_C2(12, 3, 'OOOOOO|OOXXO|XOOOOX')
False                                                    # Only two subgroup can sit at the 1st and 3rd section.
```
**Hint**: The `count` function in Python is handy for counting occurrences of a substring within a string. It's defined as `string.count(substring, start=0, end=len(string))`, where `start` and `end` parameters are optional and can specify a search range.
For more details, refer to the official Python documentation on `count`: [Python String Methods - count()](https://docs.python.org/3/library/stdtypes.html#str.count).

### C) Consider Condition 2, Print Seat Configuration.

In Part C, in addition to what you have implemented in Part B, you will return the new seat configuration after you have allocated seats for every subgroup if possible, considering **Condition 2**. Start allocating seats from the leftmost available seat. If it is not possible to allocate seats for every subgroup under these conditions, return `None`.

To achieve this, you should implement a function called ```get_seating_arrangement_C2(num_of_chars, num_of_subgroups, seat_config)``` that takes four arguments as follows:
- ```num_of_chars``` : number of characters in the group (int)
- ```num_of_subgroups``` : number of groups that they want to split into (int)
- ```seat_config``` : current seat configuration of the cinema (string)

If it is possible to find place for every subgroup considering **Condition 2**, your function should return the solution. To denote that you have allocated a seat for a cartoon character, put 'S' in that index. If there is no solution, just return `None`.

Some updated examples considering Condition 2:
```
>> get_seating_arrangement_C2(6, 3, 'OO|OX|OO|XX|XO|OO')
'SS|OX|SS|XX|XO|SS'
```
```
>> get_seating_arrangement_C2(6, 3, 'OOOO|XXOO')
'SSSS|XXSS'
```
```
>> get_seating_arrangement_C2(6, 2, 'OOO|XXO|OOO')
'SSS|XXO|SSS'
```
```
>> get_seating_arrangement_C2(4, 2, 'OOOO|OXXO')
'SSSS|OXXO'
```
```
>> get_seating_arrangement_C2(12, 3,'OOOOOO|OOXXO|XOOOOX')
None        # Not enough adjacent seats for all subgroups considering Condition 2
```
```
>> get_seating_arrangement_C2(8, 4, 'OO|OO|OX|OO')
None        # Not enough adjacent seats for all subgroups considering Condition 2
```

