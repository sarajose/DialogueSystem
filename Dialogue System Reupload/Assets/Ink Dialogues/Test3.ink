VAR trust = 0
VAR happiness = 0
VAR stable = 0
VAR trustlevel = 0

-> questions

=== questions ===
These are the available questions: #Player

* ["Did you know the victim?"]
    **[Dominant]
    ->q1a1
    **[Concienciousness]
    ->q1a2
    **[Infulence]
    
* ["How are you?"]
* ["What is your alibi?"]
* ["How are you?"]
* ["Did you know the victim?"]
* ["What is your alibi?"]
* ["How are you?"]
* ["Did you know the victim?"]
* ["What is your alibi?"]
* ["How are you?"]
* ["Did you know the victim?"]
* ["What is your alibi?"]
* ["How are you?"]
* ["Did you know the victim?"]
* ["What is your alibi?"]


-This is the end! 

-> DONE

=== q1a1 ===
~ trust = 40
{
	- trust == 0:
		<> "But surely you are not serious?" I demanded.
		
	- trust > 0:
	    ~ happiness = 5
	    ~ stable = 8
	    ~ trustlevel = 6 
		<> "But surely you are not serious?{trustlevel} " I demanded.
	- else:
	    <> "But there must be a reason for this trip," I observed.
}
->questions

=== q1a2 ===
Did you see it now did? #Dina

* ["Yes"]
* ["I still have doubts"]

- All of this is too short. Le'ts add some random text
- Another question I wanted to ask. How are you feeling?

->  DONE