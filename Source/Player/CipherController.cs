// Enhanced CipherController.cs
// Third-person player controller with advanced movement, animations, and parkour
using FlaxEngine;

public class CipherController : Script
{
    // References
    private CharacterController _characterController;
    private AnimationController _animationController;
    
    // Movement settings
    public float WalkSpeed = 2.5f;
    public float RunSpeed = 5.5f;
    public float CrouchSpeed = 1.2f;
    public float Acceleration = 15.0f;
    public float JumpForce = 8.0f;
    public float Gravity = -35.0f;
    public float GroundCheckDistance = 0.1f;
    
    // Parkour settings
    public float VaultHeight = 1.0f;
    public float VaultDistance = 1.5f;
    public float WallRunHeight = 2.0f;
    public float WallRunDistance = 1.0f;
    public float LedgeGrabHeight = 1.2f;
    public float MantleHeight = 0.8f;
    
    // State
    private Vector3 _velocity;
    private bool _isCrouching;
    private bool _isJumping;
    private bool _isVaulting;
    private bool _isWallRunning;
    private bool _isLedgeGrabbing;
    private bool _isMantling;
    private bool _isGrounded;
    
    // Animation parameters
    private const string PARAM_SPEED = "Speed";
    private const string PARAM_IS_CROUCHING = "IsCrouching";
    private const string PARAM_IS_JUMPING = "IsJumping";
    private const string PARAM_IS_VAULTING = "IsVaulting";
    private const string PARAM_IS_WALL_RUNNING = "IsWallRunning";
    private const string PARAM_IS_LEDGE_GRABBING = "IsLedgeGrabbing";
    private const string PARAM_IS_MANTLING = "IsMantling";
    
    public override void OnStart()
    {
        // Get components
        _characterController = Actor.GetComponent<CharacterController>();
        _animationController = Actor.GetComponent<AnimationController>();
        
        if (_characterController == null)
        {
            Debug.LogError("CipherController requires a CharacterController component.");
            enabled = false;
            return;
        }
        
        // Initialize animation parameters if animation controller exists
        if (_animationController != null)
        {
            // Parameters would be set up in the animation graph in the editor
            // For now, we'll just log that we found it
            Debug.Log("AnimationController found - ready for animation integration");
        }
    }
    
    public override void OnUpdate()
    {
        if (_characterController == null) return;
        
        HandleGroundCheck();
        HandleMovement();
        HandleJump();
        HandleCrouch();
        HandleParkour();
        
        // Apply gravity
        if (!_isGrounded)
            _velocity.Y += Gravity * Time.DeltaTime;
        else if (_velocity.Y < 0)
            _velocity.Y = -2f; // Small downward force to keep grounded
        
        // Move the character
        _characterController.Move(_velocity * Time.DeltaTime);
        
        // Update animations
        UpdateAnimations();
    }
    
    private void HandleGroundCheck()
    {
        // Simple ground check using character controller
        _isGrounded = _characterController.IsGrounded;
        
        // Alternative: raycast down for more precise ground check
        /*
        var ray = new Ray(_characterController.Position + Vector3.Up * 0.1f, Vector3.Down);
        if (Physics.Raycast(ray, out var hit, GroundCheckDistance))
        {
            _isGrounded = true;
            // Snap to ground if close
            if (hit.Distance < 0.01f)
                _characterController.Position = hit.Point + Vector3.Up * _characterController.StepOffset;
        }
        else
        {
            _isGrounded = false;
        }
        */
    }
    
    private void HandleMovement()
    {
        // Get input (these would be set up in Flax Input Settings)
        float moveX = Input.GetAxis("MoveX"); // A/D or left stick X
        float moveZ = Input.GetAxis("MoveY"); // W/S or left stick Y
        
        Vector3 input = new Vector3(moveX, 0, moveZ);
        input = Vector3.ClampMagnitude(input, 1.0f);
        
        // Calculate desired velocity based on state
        float speed = WalkSpeed;
        if (Input.GetButton("Run") && !_isCrouching) speed = RunSpeed;
        if (_isCrouching) speed = CrouchSpeed;
        
        // If parkour actions are active, movement might be restricted or altered
        if (_isVaulting || _isWallRunning || _isLedgeGrabbing || _isMantling)
        {
            // During parkour, we might want to maintain momentum or follow a specific path
            // For simplicity, we'll reduce player control during these actions
            speed *= 0.5f;
        }
        
        Vector3 desiredVelocity = Transform.Rotation * input * speed;
        
        // Smoothly interpolate velocity
        _velocity.XZ = Vector3.Lerp(_velocity.XZ, desiredVelocity.XZ, Acceleration * Time.DeltaTime);
    }
    
    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded && !_isCrouching && !_isVaulting && !_isWallRunning && !_isLedgeGrabbing && !_isMantling)
        {
            _velocity.Y = JumpForce;
            _isJumping = true;
        }
        
        // Reset jumping flag when we land
        if (_isGrounded && _velocity.Y <= 0)
            _isJumping = false;
    }
    
    private void HandleCrouch()
    {
        if (Input.GetButtonDown("Crouch") && _isGrounded && !_isJumping && !_isVaulting && !_isWallRunning && !_isLedgeGrabbing && !_isMantling)
        {
            _isCrouching = !_isCrouching;
            // Adjust character controller height
            if (_characterController != null)
            {
                _characterController.Height = _isCrouching ? 1.0f : 2.0f;
                // Adjust center if needed
                _characterController.Center = _isCrouching ? new Vector3(0, 0.5f, 0) : new Vector3(0, 1.0f, 0);
            }
        }
        
        // Prevent crouching while in air or during parkour
        if (!_isGrounded || _isJumping || _isVaulting || _isWallRunning || _isLedgeGrabbing || _isMantling)
        {
            _isCrouching = false;
            if (_characterController != null)
            {
                _characterController.Height = 2.0f;
                _characterController.Center = new Vector3(0, 1.0f, 0);
            }
        }
    }
    
    private void HandleParkour()
    {
        // Only attempt parkour if on ground and not already doing parkour
        if (_isGrounded && !_isJumping && !_isCrouching)
        {
            HandleVault();
            HandleWallRun();
            HandleLedgeGrab();
            HandleMantle();
        }
    }
    
    private void HandleVault()
    {
        if (Input.GetButtonDown("Vault") && !_isVaulting && !_isWallRunning && !_isLedgeGrabbing && !_isMantling)
        {
            // Raycast forward from character's position to detect low obstacle
            var start = Transform.Position + Vector3.Up * 0.5f; // Slightly above ground
            var direction = Transform.Forward;
            if (Physics.Raycast(start, direction, out var hit, VaultDistance + 0.5f))
            {
                // Check if hit is at vaultable height (not too high, not too low)
                if (hit.Distance > 0.3f && hit.Distance < VaultDistance)
                {
                    // Check if we can vault over it (check height on other side)
                    var checkPoint = hit.Point + Transform.Forward * 0.5f;
                    var checkDown = new Ray(checkPoint + Vector3.Up * 0.5f, Vector3.Down);
                    if (Physics.Raycast(checkDown, out var groundHit, 2.0f))
                    {
                        float obstacleHeight = groundHit.Point.Y - checkPoint.Y;
                        if (obstacleHeight < VaultHeight) // Obstacle is low enough to vault
                        {
                            StartVault(hit.Point);
                        }
                    }
                }
            }
        }
    }
    
    private void StartVault(Vector3 obstaclePoint)
    {
        _isVaulting = true;
        _velocity.Y = JumpForce * 0.7f; // Some upward velocity
        _velocity.Z = Transform.Forward.Z * RunSpeed * 1.5f; // Forward momentum
        _velocity.X = Transform.Forward.X * RunSpeed * 1.5f;
        
        Debug.Log("Vault started");
        
        // In a full implementation, we would:
        // 1. Play vault animation
        // 2. Use root motion or timed movement to clear obstacle
        // 3. Reset state after vault completes
        
        // For now, simulate with a timer
        Invoke(nameof(EndVault), 0.3f); // Vault duration
    }
    
    private void EndVault()
    {
        _isVaulting = false;
        Debug.Log("Vault ended");
        CancelInvoke(nameof(EndVault));
    }
    
    private void HandleWallRun()
    {
        // Simplified wall run - in reality this would be more complex
        if (Input.GetButtonDown("WallRun") && !_isWallRunning && !_isVaulting && !_isLedgeGrabbing && !_isMantling)
        {
            // Check for wall to run on
            var start = Transform.Position + Vector3.Up * 0.5f;
            var direction = Transform.Forward;
            if (Physics.Raycast(start, direction, out var hit, WallRunDistance + 0.5f))
            {
                // Check if wall is tall enough
                var checkUp = new Ray(hit.Point, Vector3.Up);
                if (Physics.Raycast(checkUp, out var topHit, WallRunHeight + 0.5f))
                {
                    StartWallRun(hit.Point, hit.Normal);
                }
            }
        }
    }
    
    private void StartWallRun(Vector3 wallPoint, Vector3 wallNormal)
    {
        _isWallRunning = true;
        // Calculate wall run direction (along the wall)
        Vector3 wallDirection = Vector3.Cross(wallNormal, Vector3.Up);
        wallDirection.Normalize();
        
        _velocity = wallDirection * RunSpeed * 1.2f; // Slightly faster than run
        _velocity.Y = JumpForce * 0.3f; // Some lift to stay on wall
        
        Debug.Log("Wall run started");
        
        // Simulate with timer
        Invoke(nameof(EndWallRun), 1.5f); // Wall run duration
    }
    
    private void EndWallRun()
    {
        _isWallRunning = false;
        Debug.Log("Wall run ended");
        CancelInvoke(nameof(EndWallRun));
    }
    
    private void HandleLedgeGrab()
    {
        if (Input.GetButtonDown("Jump") && !_isGrounded && !_isLedgeGrabbing && !_isVaulting && !_isWallRunning && !_isMantling)
        {
            // Check for ledge to grab
            var start = Transform.Position + Vector3.Up * 0.5f;
            var direction = Transform.Forward;
            if (Physics.Raycast(start, direction, out var hit, LedgeGrabHeight + 0.5f))
            {
                // Check if there's ground below the ledge
                var checkDown = new Ray(hit.Point + Vector3.Up * 0.1f, Vector3.Down);
                if (!Physics.Raycast(checkDown, out var groundHit, 2.0f)) // No ground below = ledge
                {
                    StartLedgeGrab(hit.Point);
                }
            }
        }
    }
    
    private void StartLedgeGrab(Vector3 ledgePoint)
    {
        _isLedgeGrabbing = true;
        // Position character at ledge
        Transform.Position = new Vector3(ledgePoint.X, ledgePoint.Y - 0.5f, ledgePoint.Z);
        _velocity = Vector3.Zero;
        
        Debug.Log("Ledge grab started");
        
        // Simulate with timer - player can press up to mantle or jump off
        Invoke(nameof(CheckLedgeGrabInput), 0.1f);
    }
    
    private void CheckLedgeGrabInput()
    {
        if (!_isLedgeGrabbing) return;
        
        if (Input.GetButtonDown("Jump")) // Jump off ledge
        {
            EndLedgeGrab();
            _velocity.Y = JumpForce * 0.7f;
        }
        else if (Input.GetAxis("MoveY") > 0.5f) // Move up to mantle
        {
            StartMantle();
        }
        
        Invoke(nameof(CheckLedgeGrabInput), 0.1f); // Keep checking
    }
    
    private void EndLedgeGrab()
    {
        _isLedgeGrabbing = false;
        Debug.Log("Ledge grab ended");
        CancelInvoke(nameof(CheckLedgeGrabInput));
    }
    
    private void StartMantle()
    {
        if (!_isLedgeGragging) return;
        
        _isLedgeGrabbing = false;
        _isMantling = true;
        _velocity = Vector3.Zero;
        
        Debug.Log("Mantle started");
        
        // Simulate with timer
        Invoke(nameof(EndMantle), 0.4f); // Mantle duration
    }
    
    private void EndMantle()
    {
        _isMantling = false;
        // Position character on top of the ledge
        Transform.Position = Transform.Position + Vector3.Up * MantleHeight;
        Debug.Log("Mantle ended");
        CancelInvoke(nameof(EndMantle));
    }
    
    private void UpdateAnimations()
    {
        if (_animationController == null) return;
        
        // Set animation parameters
        _animationController.SetParameter(PARAM_SPEED, _velocity.XZ.Length());
        _animationController.SetParameter(PARAM_IS_CROUCHING, _isCrouching);
        _animationController.SetParameter(PARAM_IS_JUMPING, _isJumping);
        _animationController.SetParameter(PARAM_IS_VAULTING, _isVaulting);
        _animationController.SetParameter(PARAM_IS_WALL_RUNNING, _isWallRunning);
        _animationController.SetParameter(PARAM_IS_LEDGE_GRABBING, _isLedgeGrabbing);
        _animationController.SetParameter(PARAM_IS_MANTLING, _isMantling);
        
        // In a full implementation, we would also set direction parameters for blend trees
    }
}
