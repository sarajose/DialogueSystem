VAR health = 30
VAR isMain = false
-> multioption 

=== multioption ===
I don't know them. I have never seen these people before.
* ["Are you sure about it?"]
    -> q2
* ["Tell me the truth"]
    -> q2
* ["It's fine, let's move on"]
    -> q2
=== questions ===
Hi, I have to ask you a couple of questions. Let's start whenever you are ready.
* ["Did you know any of the people involved in the robbery?"]
    -> q2
* ["Does you or your friends have an alibi?"]
    -> q2
* ["Did you know any of the people?"]
    -> q2
* ["Did you know there was a robbery?"]
    -> q2
* ["Do you like living here?"]
    -> q2
* ["Did you kill her?"]
    -> q2   
* ["Did you love her?"]
    -> q2
* ["Are you related to the victim?"]
    -> q2   
* ["What do you like for fun?"]
    -> q2
* ["Do you miss her?"]
    -> q2

=== q1 ===
~ isMain = true
Hello! #Dan
Have you seen a dialogue sytem before? 
* ["No I haven't"]
    "Oh ok"
    -> q2
* ["A what?"]
    "Dialogue system"
    -> q1 

=== q2 ===
Did you see it now did?

* ["Yes"]
* ["I still have doubts"]

- Another question I wanted to ask. How are you feeling?

* ["well"]
* ["not well"]

-This is the end! 

-> END

//markup langauage can be use in rich text